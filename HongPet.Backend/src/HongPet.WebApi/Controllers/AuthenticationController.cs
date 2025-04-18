using HongPet.Application.Commons;
using HongPet.Application.Services;
using HongPet.Application.Services.Abstractions;
using HongPet.Application.Services.Authentication;
using HongPet.Domain.Entities;
using HongPet.SharedViewModels.ResponseModel;
using HongPet.SharedViewModels.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HongPet.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthenticationController(
    IUserService _userService,
    ITokenHandler _tokenHandler,
    AppConfiguration _appConfiguration,
    IUserTokenService _userTokenService) : ControllerBase
{

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
    {
        #region verify login
        if (loginModel == null)
        {
            return BadRequest(new ApiResponse
            {
                IsSuccess = false,
                ErrorMessage = "Input không hợp lệ!"
            });
        }
        // check whether account exist in db
        var account = await _userService.CheckLoginAsync(loginModel.Email, loginModel.Password);

        if (account == null)
        {
            return Unauthorized(new ApiResponse
            {
                IsSuccess = false,
                ErrorMessage = "Tên đăng nhập hoặc mật khẩu không đúng."
            });
        }
        if (account.DeletedDate != null)
        {
            return Unauthorized(new ApiResponse
            {
                IsSuccess = false,
                ErrorMessage = "Tài khoản của bạn đã bị vô hiệu hóa."
            });
        }
        #endregion
        
        // login success - issue (access token, refresh token) pair
        var issuedDate = CurrentTime.GetCurrentTime;
        var accessTokenModel = _tokenHandler.CreateAccessToken(account, issuedDate);
        var refreshTokenModel = _tokenHandler.CreateRefreshToken(account, issuedDate);
        var token = new UserToken
        {
            UserId = account.Id,
            ATid = accessTokenModel.TokenId,
            RTid = refreshTokenModel.TokenId,
            RefreshToken = refreshTokenModel.Token,
            IssuedDate = issuedDate,
            ExpiredDate = issuedDate.AddHours(_appConfiguration.JwtConfiguration.RTExpHours),
        };
        await _userTokenService.AddAsync(token); // save token in db

        return Ok(new ApiResponse
        {
            IsSuccess = true,
            Data = new TokenVM
            {
                AccessToken = accessTokenModel.Token,
                RefreshToken = refreshTokenModel.Token,
                UserId = account.Id,
                Email = account.Email
            }
        });
    }
}
