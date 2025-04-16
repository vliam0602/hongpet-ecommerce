using Domain.Entities.Commons;
using HongPet.Domain.Repositories.Abstraction.Commons;

namespace HongPet.Domain.Repositories.Abstractions.Commons;
public interface IUnitOfWork : IDisposable
{
    IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity;
    Task<int> SaveChangesAsync();
}
