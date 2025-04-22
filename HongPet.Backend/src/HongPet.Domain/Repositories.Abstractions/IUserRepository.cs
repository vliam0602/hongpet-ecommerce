using HongPet.Domain.Entities;

namespace HongPet.Domain.Repositories.Abstractions;
public interface IUserRepository : IGenericRepository<User>
{
    Task<bool> IsEmailExistAsync(string email);
}
