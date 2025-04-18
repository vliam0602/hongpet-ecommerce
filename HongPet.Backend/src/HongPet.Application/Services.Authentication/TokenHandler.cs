using HongPet.Application.Commons;
using HongPet.Application.Models;
using HongPet.Application.Services.Abstractions;
using HongPet.Domain.Entities;
using HongPet.SharedViewModels.ViewModels;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HongPet.Application.Services.Authentication;
public class TokenHandler : ITokenHandler
{
    private readonly AppConfiguration _appConfig;
    private readonly IUserTokenService _userTokenService;
    public TokenHandler(AppConfiguration appConfig,
        IUserTokenService userTokenService)
    {
        _appConfig = appConfig;
        _userTokenService = userTokenService;
    }

    public TokenModel CreateAccessToken(User user, DateTime issueTime)
    {
        var jwtConfig = _appConfig.JwtConfiguration;

        var jti = NewId.Next().ToGuid();

        var expTime = issueTime.AddHours(jwtConfig.ATExpHours);

        var tokenStr = GenerateToken(user, jti, issueTime, expTime);

        return new TokenModel { Token = tokenStr, TokenId = jti };
    }

    public TokenModel CreateRefreshToken(User user, DateTime issueTime)
    {
        var jwtConfig = _appConfig.JwtConfiguration;

        var jti = NewId.Next().ToGuid();

        var expTime = issueTime.AddHours(jwtConfig.RTExpHours);

        var tokenStr = GenerateToken(user, jti, issueTime, expTime);

        return new TokenModel { Token = tokenStr, TokenId = jti };
    }    
    
    public async Task ValidateAccessTokenAsync(TokenValidatedContext context)
    {
        var identity = context.Principal?.Identity as ClaimsIdentity;
        var jtiClaim = identity?.FindFirst(JwtRegisteredClaimNames.Jti);

        // check if token existed in db 
        // remove the token when logout
        Guid jti;
        if (!Guid.TryParse(jtiClaim!.Value, out jti))
        {
            context.Fail("Invalid token id!");
            return;
        }

        var userToken = (await _userTokenService
                            .GetAsync(x => x.RTid == jti))
                            .SingleOrDefault();            

        if (userToken == null)
        {
            context.Fail("Invalid token! Token id does not exist.");            
        }        
    }

    public async Task<TokenVM?> ValidateRefreshTokenAsync(string refreshToken)
    {
        try
        {
            #region validate the token
            // Decode the token
            var jwtConfig = _appConfig.JwtConfiguration;

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtConfig.Issuer,
                ValidAudience = jwtConfig.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtConfig.SecretKey))
            };

            // Validate token and extract claims
            var principal = new JwtSecurityTokenHandler()
                .ValidateToken(refreshToken, validationParameters, out var validatedToken);

            if (validatedToken is not JwtSecurityToken jwtToken ||
                !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            var jtiClaim = principal.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;

            if (string.IsNullOrEmpty(jtiClaim)
                    || !Guid.TryParse(jtiClaim, out var jti))
            {
                throw new SecurityTokenException("Invalid token ID");
            }

            // Check token existence in the database
            var userToken = (await _userTokenService
                .GetAsync(x => x.RTid == jti))
                .SingleOrDefault();

            if (userToken == null || userToken.RefreshToken != refreshToken)
            {
                throw new SecurityTokenException("Token not found or mismatched");
            }

            // Check token expiration
            if (userToken.ExpiredDate < DateTime.UtcNow)
            {
                throw new SecurityTokenException("Token has expired");
            }
            #endregion

            #region generate new tokens
            // Generate new access token and refresh token
            var issueTime = CurrentTime.GetCurrentTime;
            var newAccessToken =
                GenerateToken(
                    user: userToken.User,
                    jti: NewId.Next().ToGuid(),
                    issueTime: issueTime,
                    expTime: issueTime.AddHours(jwtConfig.ATExpHours)
                );
            var newRefreshToken =
                GenerateToken(
                    user: userToken.User,
                    jti: NewId.Next().ToGuid(),
                    issueTime: issueTime,
                    expTime: issueTime.AddHours(jwtConfig.RTExpHours)
                );

            // Delete the old user token
            await _userTokenService.DeleteAsync(userToken.Id); // hard delete

            // Save the new refresh token in the database
            var newUserToken = new UserToken
            {
                UserId = userToken.UserId,
                ATid = Guid.NewGuid(),
                RTid = Guid.NewGuid(),
                RefreshToken = newRefreshToken,
                IssuedDate = DateTime.UtcNow,
                ExpiredDate = DateTime.UtcNow.AddHours(jwtConfig.RTExpHours)
            };

            await _userTokenService.AddAsync(newUserToken);
            #endregion

            // Return the new tokens
            return new TokenVM
            {
                UserId = userToken.UserId,
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
                Email = userToken.User.Email
            };
        } catch (Exception ex)
        {
            throw new Exception($"Error validating refresh token: {ex.Message}");
        }
    }

    private string GenerateToken(User user, Guid jti, 
        DateTime issueTime, DateTime expTime)
    {
        #region config the JWT security
        var jwtConfig = _appConfig.JwtConfiguration;

        var securityKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtConfig.SecretKey));

        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        #endregion

        #region register the claims        

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Jti, jti.ToString()),
            new Claim(JwtRegisteredClaimNames.Iss, jwtConfig.Issuer),
            new Claim(JwtRegisteredClaimNames.Aud, jwtConfig.Audience),
            //dd/mm/yyyy or mm/dd/yyy depent on culture
            new Claim(JwtRegisteredClaimNames.Iat,issueTime.ToString("G")),
            new Claim(JwtRegisteredClaimNames.Exp, expTime.ToString("G")),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Fullname),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };
        #endregion

        var token = new JwtSecurityToken(
                claims: claims,
                notBefore: issueTime,
                expires: expTime,
                signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
