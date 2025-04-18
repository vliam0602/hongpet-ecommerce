using HongPet.Application.Commons;
using HongPet.Domain.Entities;
using System.Security.Principal;

namespace HongPet.Application.Services.Abstractions;
public interface IUserService : IGenericService<User>
{
    Task<User?> CheckLoginAsync(string email, string password);
    Task CreateNewUserAsync(User user);
    Task EditPasswordAsync(Guid userId, string oldPassword, string newPassword);
}
