using Domain.Entities.Commons;

namespace HongPet.Domain.Repositories.Abstractions;
public interface IUnitOfWork : IDisposable
{
    // generic for all repositories
    IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity;
    // repository that have specific logic
    IProductRepository ProductRepository { get; }
    IUserRepository UserRepository { get; }
    IReviewRepository ReviewRepository { get; }
    IOrderRepository OrderRepository { get; }
    Task<int> SaveChangesAsync();
}
