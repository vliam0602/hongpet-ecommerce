using AutoMapper;
using AutoMapper.QueryableExtensions;
using HongPet.Domain.Entities;
using HongPet.Domain.Repositories.Abstractions;
using HongPet.Infrastructure.Database;
using HongPet.Infrastructure.DTOs;
using HongPet.Infrastructure.Repositories.Commons;
using HongPet.SharedViewModels.Generals;
using Microsoft.EntityFrameworkCore;

namespace HongPet.Infrastructure.Repositories;
public class OrderRepository : GenericRepository<Order>, IOrderRepository
{
    private IMapper _mapper;
    public OrderRepository(AppDbContext context, IMapper mapper) : base(context)
    {
        _mapper = mapper;
    }

    public async Task<IPagedList<OrderDto>> GetOrderByCustomerIdAsync(Guid customerId, 
        int pageIndex = 1, int pageSize = 10, string searchKey = "")
    {
        //var orders = _dbSet
        //    .Include(x => x.OrderItems)
        //        .ThenInclude(o => o.Variant)
        //            .ThenInclude(v => v.AttributeValues)
        //                .ThenInclude(a => a.Attribute)
        //    .Where(o => o.CustomerId == customerId);

        var orders = _dbSet
            .Where(x => x.CustomerId == customerId)
            .ProjectTo<OrderDto>(_mapper.ConfigurationProvider)
            .OrderByDescending(x => x.CreatedDate);

        return await this.ToPaginAsync(orders, pageIndex, pageSize);
    }

    public async Task<OrderDto?> GetOrderDetailAsync(Guid id)
    {
        //return await _dbSet.Include(x => x.OrderItems)
        //                    .ThenInclude(o => o.Variant)
        //                        .ThenInclude(v => v.AttributeValues)
        //                            .ThenInclude(a => a.Attribute)
        //                    .SingleOrDefaultAsync(o => o.Id == id);

        return await _dbSet.Where(x => x.Id == id)
            .ProjectTo<OrderDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync();
    }

    private async Task<IPagedList<OrderDto>> ToPaginAsync(
        IQueryable<OrderDto> entities, int pageIndex = 1, int pageSize = 10)
    {
        var totalCount = entities.Count();

        var items = await entities.Skip((pageIndex - 1) * pageSize)
                            .Take(pageSize)
                            .ToListAsync();

        return new PagedList<OrderDto>(items, totalCount, pageIndex, pageSize);
    }
}
