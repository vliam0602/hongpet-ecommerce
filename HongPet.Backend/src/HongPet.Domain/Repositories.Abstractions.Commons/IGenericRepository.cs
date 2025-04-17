
using Domain.Entities.Commons;
using System.Linq.Expressions;

namespace HongPet.Domain.Repositories.Abstraction.Commons;
public interface IGenericRepository<TEntity> where TEntity : BaseEntity
{
    Task<TEntity?> GetByIdAsync(Guid id);
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task AddAsync(TEntity entity);
    void Update(TEntity entity);
    Task DeleteAsync(Guid id);
    Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> query);
    Task<IPagedList<TEntity>> GetPagedAsync(int pageIndex, int pageSize);
}
