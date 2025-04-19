using Domain.Entities.Commons;
using HongPet.Domain.Repositories.Abstraction.Commons;

namespace HongPet.Domain.Repositories.Abstractions.Commons;
public interface IUnitOfWork : IDisposable
{
    // generic for all repositories
    IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity;
    // repository that have specific logic
    IProductRepository ProductRepository { get; }
    IUserRepository UserRepository { get; }
    Task<int> SaveChangesAsync();
}
