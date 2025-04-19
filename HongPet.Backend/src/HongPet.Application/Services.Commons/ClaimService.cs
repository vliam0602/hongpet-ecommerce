using HongPet.Domain.Enums;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace HongPet.Application.Services.Commons;
public class ClaimService(
    IHttpContextAccessor _httpContextAccessor) : IClaimService
{
    public Guid? GetCurrentUserId
    {
        get
        {
            var userIdClaim = GetClaimValue(ClaimTypes.NameIdentifier);

            return string.IsNullOrEmpty(userIdClaim) ?
                            null : Guid.Parse(userIdClaim);
        }
    }
    public string? GetCurrentEmail => GetClaimValue(ClaimTypes.Email);            

    public string? GetCurrentRole => GetClaimValue(ClaimTypes.Role);

    public bool? IsAdmin()
    {
        var role = GetCurrentRole;
        return string.IsNullOrEmpty(role) ?
                            null : role.Equals(RoleEnum.Admin.ToString());
    }

    private string? GetClaimValue(string claimType)
    {
        var claimValue = _httpContextAccessor
                            .HttpContext?
                            .User?
                            .FindFirstValue(claimType);

        return string.IsNullOrEmpty(claimValue) ?
                        null : claimValue;
    }
}
