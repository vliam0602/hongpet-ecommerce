using HongPet.Application.Commons;
using HongPet.Domain.Entities;

namespace HongPet.Application.Services.Abstractions;
public interface IUserService : IGenericService<User>
{
    Task<User?> GetByEmailAndPassword(string email, string password);
    Task CreateNewAccount(string email, string password);

}
