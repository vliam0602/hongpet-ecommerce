using HongPet.Application.Commons;
using HongPet.Domain.Entities.Commons;
using HongPet.Domain.Repositories.Abstractions;
using HongPet.SharedViewModels.Generals;
using System.Linq.Expressions;

namespace HongPet.Application.Services.Commons;

public class GenericService<TEntity>
    : IGenericService<TEntity> where TEntity : BaseEntity
{
    protected readonly IUnitOfWork _unitOfWork;
    protected IGenericRepository<TEntity> _repository;
    protected IClaimService _claimService;

    public GenericService(IUnitOfWork unitOfWork, IClaimService claimService)
    {
        _unitOfWork = unitOfWork;
        _repository = _unitOfWork.Repository<TEntity>();
        _claimService = claimService;
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
        entity.CreatedBy = _claimService.GetCurrentUserId;   
        entity.CreatedDate = CurrentTime.GetCurrentTime;

        entity.LastModificatedBy = _claimService.GetCurrentUserId;
        entity.LastModificatedDate = CurrentTime.GetCurrentTime;

        await _repository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();
    }

    public virtual async Task UpdateAsync(TEntity entity)
    {
        entity.LastModificatedBy = _claimService.GetCurrentUserId;
        entity.LastModificatedDate = CurrentTime.GetCurrentTime;

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

    public async Task SoftDeleteAsync(Guid id)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new KeyNotFoundException($"Entity with the id {id} not found.");
        }
        if (entity.DeletedDate != null)
        {
            throw new ArgumentException($"Entity with the id {id} has been deleted.");
        }
        entity.DeletedBy = _claimService.GetCurrentUserId;
        entity.DeletedDate = CurrentTime.GetCurrentTime;

        _repository.Update(entity);
        await _unitOfWork.SaveChangesAsync();
    }
}
