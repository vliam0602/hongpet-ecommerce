using HongPet.Application.Models;
using HongPet.Domain.Entities;
using HongPet.SharedViewModels.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace HongPet.Application.Services.Authentication;
public interface ITokenHandler
{
    TokenModel CreateAccessToken(User user, DateTime issueTime);
    TokenModel CreateRefreshToken(User user, DateTime issueTime);
    Task ValidateAccessTokenAsync(TokenValidatedContext context);
    Task<TokenVM?> ValidateRefreshTokenAsync(string refreshToken);
}
