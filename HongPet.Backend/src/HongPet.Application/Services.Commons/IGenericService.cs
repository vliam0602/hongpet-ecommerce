using HongPet.Domain.Entities.Commons;
using HongPet.SharedViewModels.Generals;
using System.Linq.Expressions;

namespace HongPet.Application.Commons
{
    public interface IGenericService<TEntity> where TEntity: BaseEntity
    {
        Task<TEntity?> GetByIdAsync(Guid id);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task AddAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(Guid id);
        Task SoftDeleteAsync(Guid id);
        Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> query);
        Task<IPagedList<TEntity>> GetPagedAsync(int pageIndex = 1, int pageSize = 10, string? keyword="");
    }
}
