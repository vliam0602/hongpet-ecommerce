using HongPet.Domain.Entities;
using HongPet.Domain.Repositories.Abstraction.Commons;

namespace HongPet.Domain.Repositories.Abstractions;
public interface IUserRepository : IGenericRepository<User>
{
    Task<bool> IsEmailExistAsync(string email);
}
