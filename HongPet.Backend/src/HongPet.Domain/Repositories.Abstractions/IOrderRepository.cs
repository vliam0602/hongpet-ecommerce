using Domain.Entities.Commons;
using HongPet.Domain.Entities;

namespace HongPet.Domain.Repositories.Abstractions;
public interface IOrderRepository : IGenericRepository<Order>
{
    Task<IPagedList<Order>> GetOrderByCustomerIdAsync(Guid customerId, 
        int pageIndex = 1, int pageSize = 10, string searchKey = "");
    Task<Order?> GetOrderDetailAsync(Guid id);
}
