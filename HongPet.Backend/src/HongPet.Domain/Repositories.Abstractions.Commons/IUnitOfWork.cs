using Domain.Entities.Commons;
using HongPet.Domain.Repositories.Abstraction.Commons;

namespace HongPet.Domain.Repositories.Abstractions.Commons;
public interface IUnitOfWork : IDisposable
{
    // generic for all repositories
    IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity;

    // custom repositories
    IProductRepository ProductRepository { get; }
    IUserTokenRepository UserTokenRepository { get; }


    Task<int> SaveChangesAsync();
}
