using HongPet.Application.Commons;
using HongPet.Domain.Entities;
using HongPet.SharedViewModels.Generals;
using HongPet.SharedViewModels.Models;
using HongPet.SharedViewModels.ViewModels;

namespace HongPet.Application.Services.Abstractions;
public interface IUserService : IGenericService<User>
{
    Task<User?> GetByEmailAndPasswordAsync(string email, string password);
    Task CreateNewAccountAsync(string email, string password);
    Task<IPagedList<UserVM>> GetUsersListAsync(int pageIndex = 1, int pageSize = 10, string? keyword = "");
    Task<UserVM> GetUserDetailAsync(Guid id);
    Task<UserVM> UpdateUserInfoAsync(Guid id, UserUpdateModel userModel);
    Task InactiveUserAsync(Guid id); // softdelete
    Task<bool> ChangePasswordAsync(Guid id, string oldPassword, string newPassword);
    Task<bool> UpdateAvatarAsync(Guid id, string avatarUrl);

}
