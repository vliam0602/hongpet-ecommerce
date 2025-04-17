using Domain.Entities.Commons;
using System.Linq.Expressions;

namespace HongPet.Application.Commons
{
    public interface IGenericService<TEntity, TEntityVM> where TEntity: BaseEntity
    {
        Task<TEntityVM?> GetByIdAsync(Guid id);
        Task<IEnumerable<TEntityVM>> GetAllAsync();
        Task AddAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(Guid id);
        Task<IEnumerable<TEntityVM>> GetAsync(Expression<Func<TEntity, bool>> query);
        Task<PagedList<TEntityVM>> GetPagedAsync(int pageIndex, int pageSize);
    }
}
