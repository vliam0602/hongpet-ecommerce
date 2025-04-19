using HongPet.Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;


namespace HongPet.Application.Commons;
public static class GenerateJwtString
{
    public static string GenerateJwt(this User user, string secretKey, DateTime expDate)
    {
        var securityKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(secretKey));

        var credentials = new SigningCredentials(
                securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier ,user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };

        var token = new JwtSecurityToken(
            claims: claims,
            expires: expDate,
            signingCredentials: credentials);


        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
