using HongPet.Application.AppConfigurations;
using HongPet.Application.Commons;
using HongPet.Application.Services.Commons;
using HongPet.Domain.Entities;
using HongPet.SharedViewModels.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HongPet.WebApi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AuthController(
    AppConfiguration _appConfig,
    IClaimService claimService) : ControllerBase
{
    private readonly JwtConfiguration _jwtConfig = _appConfig.JwtConfiguration;
    [HttpGet("login")]
    public IActionResult Login()
    {
        var user = new User
        {
            Email = "Liam@example.com",
            
        };
        var token = user.GenerateJsonWebToken(_jwtConfig.SecretKey, CurrentTime.GetCurrentTime.AddMinutes(2));

        return Ok(new ApiResponse
        {
            Message = "Login successful",
            Data = token
        });
    }

    [HttpGet("refresh-token")]
    [Authorize]
    public IActionResult RefreshToken()
    {
        return Ok($"Refresh successful" +
            $"\\n {claimService.GetCurrentEmail}");
    }
}
