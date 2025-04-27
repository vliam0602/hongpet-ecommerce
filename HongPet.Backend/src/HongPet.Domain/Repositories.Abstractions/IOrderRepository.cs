using HongPet.Domain.Entities;
using HongPet.SharedViewModels.Generals;

namespace HongPet.Domain.Repositories.Abstractions;
public interface IOrderRepository : IGenericRepository<Order>
{
    Task<IPagedList<Order> > GetOrderByCustomerIdAsync(Guid customerId, 
        int pageIndex = 1, int pageSize = 10, string searchKey = "");
    Task<Order?> GetOrderDetailAsync(Guid id);
}
