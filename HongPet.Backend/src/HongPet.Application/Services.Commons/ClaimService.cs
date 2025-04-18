using HongPet.Domain.Enums;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace HongPet.Application.Services.Commons;
public class ClaimService : IClaimService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ClaimService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    public Guid? GetCurrentUserId
    {
        get
        {
            var userIdClaim = GetClaimValue(ClaimTypes.NameIdentifier);
            return string.IsNullOrEmpty(userIdClaim) ?
                            null : Guid.Parse(userIdClaim);
        }
    }
    public string GetCurrentEmail
    {
        get
        {
            var userNameClaim = GetClaimValue(ClaimTypes.Email);
            return string.IsNullOrEmpty(userNameClaim) ?
                            string.Empty : userNameClaim;
        }
    }

    public string GetCurrentRole
    {
        get
        {
            string userRoleClaim = GetClaimValue(ClaimTypes.Role);
            return string.IsNullOrEmpty(userRoleClaim) ?
                            string.Empty : userRoleClaim;
        }
    }

    public bool IsAdmin()
    {
        var currentRole = GetCurrentRole;
        return currentRole.Equals(RoleEnum.Admin.ToString());
    }

    private string GetClaimValue(string claimType)
    {
        var claim = _httpContextAccessor
                        .HttpContext?
                        .User?
                        .FindFirst(claimType);
        return claim?.Value ?? string.Empty;
    }
}
