using HongPet.Domain.DTOs;
using HongPet.Domain.Entities;
using HongPet.SharedViewModels.Generals;

namespace HongPet.Domain.Repositories.Abstractions;
public interface IUserRepository : IGenericRepository<User>
{
    Task<bool> IsEmailExistAsync(string email);
    Task<IPagedList<UserDto>> GetCustomersPagedAsync(
        int pageIndex = 1, int pageSize = 10, string? keyword = "");
}
