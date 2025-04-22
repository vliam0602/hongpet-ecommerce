using Domain.Entities.Commons;
using HongPet.Application.Commons;
using HongPet.Domain.Repositories.Abstractions;
using System.Linq.Expressions;

namespace HongPet.Application.Services.Commons;

public class GenericService<TEntity>
    : IGenericService<TEntity> where TEntity : BaseEntity
{
    protected readonly IUnitOfWork _unitOfWork;
    protected IGenericRepository<TEntity> _repository;

    public GenericService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _repository = _unitOfWork.Repository<TEntity>();
    }

    public virtual async Task<TEntity?> GetByIdAsync(Guid id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public virtual async Task AddAsync(TEntity entity)
    {
        await _repository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();
    }

    public virtual async Task UpdateAsync(TEntity entity)
    {
        _repository.Update(entity);
        await _unitOfWork.SaveChangesAsync();
    }

    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();
    }

    public virtual async Task<IEnumerable<TEntity>> GetAsync(
        Expression<Func<TEntity, bool>> query)
    {
        return await _repository.GetAsync(query);
    }

    public virtual async Task<IPagedList<TEntity>> GetPagedAsync(
        int pageIndex = 1, int pageSize = 10, string? keyword = "")
    {
        return await _repository.GetPagedAsync(pageIndex, pageSize, keyword);
    }
}
