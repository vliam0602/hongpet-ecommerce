using HongPet.CustomerMVC.Services.Abstraction;

namespace HongPet.CustomerMVC.Services;

public class ClaimService(
    IHttpContextAccessor httpContextAccessor) : IClaimService
{
    public bool IsAuthorized
        => httpContextAccessor.HttpContext?.Session
        .GetString(AppConstant.AccessToken) != null;

    public string AccessToken 
        => httpContextAccessor.HttpContext?.Session
            .GetString(AppConstant.AccessToken) ?? string.Empty;

    public string RefreshToken
        => httpContextAccessor.HttpContext?.Session
            .GetString(AppConstant.RefreshToken) ?? string.Empty;

    public string UserId 
        => httpContextAccessor.HttpContext?.Session
            .GetString(AppConstant.CurrentUserId) ?? string.Empty;

    public string Email 
        => httpContextAccessor.HttpContext?.Session
            .GetString(AppConstant.CurrentEmail) ?? string.Empty;

    public string Username 
        => httpContextAccessor.HttpContext?.Session
            .GetString(AppConstant.CurrentUsername) ?? string.Empty;
}
