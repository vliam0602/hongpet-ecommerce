using AutoMapper;
using HongPet.CustomerMVC.Models;
using HongPet.CustomerMVC.Services.Abstraction;
using HongPet.SharedViewModels.Models;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace HongPet.CustomerMVC.Controllers;
public class AuthController(
    ILogger<AuthController> _logger,
    IAuthApiService authApiService,
    IMapper _mapper,
    IClaimService _claimService) : Controller
{

    [HttpGet]
    public IActionResult Login()
    {
        if (_claimService.IsAuthorized)
        {
            return RedirectToAction("Index", "Home");
        }
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginModel loginModel)
    {
        if (!ModelState.IsValid)
        {
            return View(loginModel); // Trả về view với thông báo lỗi validation
        }

        try
        {
            var token = await authApiService.LoginAsync(loginModel);

            // Save token in session
            HttpContext.Session.SetString(AppConstant.AccessToken, token.AccessToken);
            HttpContext.Session.SetString(AppConstant.RefreshToken, token.RefreshToken);

            // Decode the Access Token to extract user information
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token.AccessToken);

            // Extract claims
            var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var email = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var role = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            // Save user information in session or cookies
            HttpContext.Session.SetString(AppConstant.CurrentUserId,
                userId ?? string.Empty);
            HttpContext.Session.SetString(AppConstant.CurrentEmail,
                email ?? string.Empty);
            HttpContext.Session.SetString(AppConstant.CurrentRole,
                role ?? string.Empty);

            // Redirect to Home when successful
            return RedirectToAction("Index", "Home");
        } catch (UnauthorizedAccessException ex)
        {
            ViewBag.ErrMsg = ex.Message;
            return View(loginModel);
        } catch (Exception ex)
        {
            _logger.LogError(ex, $"Unhandled exception: {ex.Message}");
            return RedirectToAction("Error", "Home", 
                new { errMsg = ex.Message });
        }
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult Register()
    {
        if (_claimService.IsAuthorized)
        {
            return RedirectToAction("Index", "Home");
        }
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel registerVM)
    {
        if (!ModelState.IsValid)
        {
            return View(registerVM);
        }

        if (registerVM.Password != registerVM.ConfirmPassword)
        {
            ViewBag.ErrMsg = "Mật khẩu xác nhận không khớp với mật khẩu!";
            return View(registerVM);
        }

        try
        {
            var registerModel = _mapper.Map<RegisterModel>(registerVM);
            // Gọi API thông qua AuthApiService
            var registeredUser = await authApiService.RegisterAsync(registerModel);

            // Đăng ký thành công, chuyển hướng đến trang đăng nhập
            TempData["SuccessMessage"] = "Đăng ký thành công! Vui lòng đăng nhập.";
            return RedirectToAction("Login");
        } catch (ArgumentException ex)
        {
            // Lỗi từ API (ví dụ: email đã tồn tại)
            ViewBag.ErrMsg = ex.Message;
            return View(registerVM);
        } catch (Exception ex)
        {
            // Lỗi không mong muốn
            _logger.LogError(ex, "Unhandled exception during registration.");
            return RedirectToAction("Error", "Home", new { errMsg = ex.Message });
        }
    }

}
