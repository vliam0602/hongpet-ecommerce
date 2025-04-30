using HongPet.CustomerMVC.Services.Abstraction;
using HongPet.CustomerMVC.Utilities;

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

    public Guid? UserId
    {
        get
        {
            var userId = httpContextAccessor.HttpContext?.Session
                .GetString(AppConstant.CurrentUserId);
            return string.IsNullOrEmpty(userId) ? null : Guid.Parse(userId);
        }
    }

    public string Email 
        => httpContextAccessor.HttpContext?.Session
            .GetString(AppConstant.CurrentEmail) ?? string.Empty;

    public string Username 
        => httpContextAccessor.HttpContext?.Session
            .GetString(AppConstant.CurrentUsername) ?? string.Empty;
}
