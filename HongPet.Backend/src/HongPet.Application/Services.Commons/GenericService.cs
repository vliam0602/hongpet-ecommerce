using AutoMapper;
using Domain.Entities.Commons;
using HongPet.Application.Commons;
using HongPet.Domain.Repositories.Abstraction.Commons;
using HongPet.Domain.Repositories.Abstractions.Commons;
using System.Linq.Expressions;

namespace HongPet.Application.Services.Commons;

public class GenericService<TEntity, TEntityVM>
    : IGenericService<TEntity, TEntityVM> where TEntity : BaseEntity
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGenericRepository<TEntity> _repository;
    private readonly IMapper _mapper;

    public GenericService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _repository = _unitOfWork.Repository<TEntity>();
        _mapper = mapper;
    }

    public async Task<TEntityVM?> GetByIdAsync(Guid id)
    {
        var entity = await _repository.GetByIdAsync(id);
        return entity != null ? _mapper.Map<TEntityVM>(entity) : default;
    }

    public async Task<IEnumerable<TEntityVM>> GetAllAsync()
    {
        var entities = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<TEntityVM>>(entities);
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

    public async Task<IEnumerable<TEntityVM>> GetAsync(Expression<Func<TEntity, bool>> query)
    {
        var entities = await _repository.GetAsync(query);
        return _mapper.Map<IEnumerable<TEntityVM>>(entities);
    }

    public async Task<PagedList<TEntityVM>> GetPagedAsync(int pageIndex, int pageSize)
    {
        var pagedItems = await _repository.GetPagedAsync(pageIndex, pageSize);
        return _mapper.Map<PagedList<TEntityVM>>(pagedItems);        
    }
}
