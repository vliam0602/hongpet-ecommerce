using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities.Commons;
using HongPet.Application.Commons;
using HongPet.Domain.Entities;
using HongPet.Domain.Repositories.Abstractions;
using HongPet.Infrastructure.Database;
using HongPet.Infrastructure.Repositories.Commons;
using HongPet.SharedViewModels.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace HongPet.Infrastructure.Repositories;
public class OrderRepository : GenericRepository<Order>, IOrderRepository
{
    private IMapper _mapper;
    public OrderRepository(AppDbContext context, IMapper mapper) : base(context)
    {
        _mapper = mapper;
    }

    public async Task<IPagedList<Order>> GetOrderByCustomerIdAsync(Guid customerId, 
        int pageIndex = 1, int pageSize = 10, string searchKey = "")
    {
        var orders = _dbSet
            .Include(x => x.OrderItems)
                .ThenInclude(o => o.Variant)
                    .ThenInclude(v => v.AttributeValues)
                        .ThenInclude(a => a.Attribute)
            .Where(o => o.CustomerId == customerId);

        return await base.ToPaginationAsync(orders, pageIndex, pageSize, searchKey);
    }

    public async Task<Order?> GetOrderDetailAsync(Guid id)
    {
        return await _dbSet.Include(x => x.OrderItems)
                            .ThenInclude(o => o.Variant)
                                .ThenInclude(v => v.AttributeValues)
                                    .ThenInclude(a => a.Attribute)
                            .SingleOrDefaultAsync(o => o.Id == id);
    }
}
