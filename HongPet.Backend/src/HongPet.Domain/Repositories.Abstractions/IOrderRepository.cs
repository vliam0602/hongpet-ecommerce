using HongPet.Domain.Entities;
using HongPet.Infrastructure.DTOs;
using HongPet.SharedViewModels.Generals;

namespace HongPet.Domain.Repositories.Abstractions;
public interface IOrderRepository : IGenericRepository<Order>
{
    Task<IPagedList<OrderDto> > GetOrderByCustomerIdAsync(Guid customerId, 
        int pageIndex = 1, int pageSize = 10, string searchKey = "");
    Task<OrderDto?> GetOrderDetailAsync(Guid id);
}
