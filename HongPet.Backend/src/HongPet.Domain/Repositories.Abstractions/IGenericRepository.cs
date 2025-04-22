
using Domain.Entities.Commons;
using System.Linq.Expressions;

namespace HongPet.Domain.Repositories.Abstractions;
public interface IGenericRepository<TEntity> where TEntity : BaseEntity
{
    Task<TEntity?> GetByIdAsync(Guid id);
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task AddAsync(TEntity entity);
    void Update(TEntity entity);
    Task DeleteAsync(Guid id);
    Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> query);
    Task<IPagedList<TEntity>> GetPagedAsync(int pageIndex=1, int pageSize=10, string? keyword="");
    Task<IPagedList<TEntity>> ToPaginationAsync(IQueryable<TEntity> entities,
        int pageIndex = 1, int pageSize = 10, string? keyword = "");
}
