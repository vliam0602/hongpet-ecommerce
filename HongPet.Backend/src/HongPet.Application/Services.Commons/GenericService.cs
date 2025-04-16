using Domain.Entities.Commons;
using HongPet.Application.Commons;
using HongPet.Domain.Repositories.Abstraction.Commons;
using HongPet.Domain.Repositories.Abstractions.Commons;
using System.Linq.Expressions;

namespace HongPet.Application.Services.Commons;
public class GenericService<TEntity> : IGenericService<TEntity> where TEntity : BaseEntity
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGenericRepository<TEntity> _repository;

    public GenericService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _repository = _unitOfWork.Repository<TEntity>();
    }

    public async Task<TEntity?> GetByIdAsync(Guid id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task AddAsync(TEntity entity)
    {
        await _repository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateAsync(TEntity entity)
    {
        _repository.Update(entity);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<IEnumerable<TEntity>> GetAsync
        (Expression<Func<TEntity, bool>> query)
    {
        return await _repository.GetAsync(query);
    }

    public async Task<(IEnumerable<TEntity> Items, int TotalCount)> GetPagedAsync
        (int pageIndex, int pageSize)
    {
        return await _repository.GetPagedAsync(pageIndex, pageSize);
    }
}
