using HongPet.Application.AppConfigurations;
using HongPet.Application.Commons;
using HongPet.Application.Services.Abstractions;
using HongPet.Application.Services.Commons;
using HongPet.Domain.Entities;
using HongPet.SharedViewModels.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HongPet.WebApi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AuthController(
    AppConfiguration _appConfig,
    IClaimService _claimService,
    IUserService _userService) : ControllerBase
{
    private readonly JwtConfiguration _jwtConfig = _appConfig.JwtConfiguration;

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
    {
        try
        {
            // verify login
            var account = await _userService
                .GetByEmailAndPassword(loginModel.Email, loginModel.Password);

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

            // login success -> issue (access token, refresh token)
            var tokens = GenerateTokens(account);

            return Ok(new ApiResponse
            {
                Message = "Đăng nhập thành công",
                Data = tokens
            });
        } catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse
            {
                ErrorMessage = $"Unexpected error: {ex.Message}"
            });
        }
    }

    [HttpGet("refresh-token")]
    [Authorize]
    public async Task<IActionResult> RefreshToken()
    {
        try
        {
            // refresh token was verified in jwt bearer middleware
            // refresh token valid -> issue new tokens
            var userId = _claimService.GetCurrentUserId;

            if (userId == null)
            {
                return Unauthorized(new ApiResponse
                {
                    IsSuccess = false,
                    ErrorMessage = "Refresh token không hợp lệ."
                });
            }

            var user = await _userService.GetByIdAsync(userId.Value);

            if (user == null)
            {
                return NotFound(new ApiResponse
                {
                    IsSuccess = false,
                    ErrorMessage = "Không tồn tại user với token hiện tại."
                });
            }

            var tokens = GenerateTokens(user);

            return Ok(new ApiResponse
            {
                Message = "Refresh token successful",
                Data = tokens
            });
        } catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse
            {
                ErrorMessage = $"Unexpected error: {ex.Message}"
            });
        }
    }

    [HttpPut("register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel registerModel)
    {
        try
        {
            await _userService.CreateNewAccount(registerModel.Email, registerModel.Password);

            return CreatedAtAction("Register", new { Email = registerModel.Email}, 
                new ApiResponse
                {
                    Message = "Đăng ký tài khoản thành công",
                    Data = registerModel
                });            
        } 
        catch (ArgumentException ex)
        {
            return BadRequest(new ApiResponse
            {
                ErrorMessage = ex.Message
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse
            {
                ErrorMessage = $"Unexpected error: {ex.Message}"
            });
        }
    }

    
    private TokenModel GenerateTokens(User user)
    {
        var issueDate = CurrentTime.GetCurrentTime;

        var accessToken = user.GenerateJwt(
            _jwtConfig.SecretKey, issueDate.AddHours(_jwtConfig.ATExpHours));

        var refreshToken = user.GenerateJwt(
            _jwtConfig.SecretKey, issueDate.AddHours(_jwtConfig.RTExpHours));

        return new TokenModel
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }
}
