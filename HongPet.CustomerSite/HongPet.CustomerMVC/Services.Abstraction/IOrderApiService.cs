using HongPet.SharedViewModels.Generals;
using HongPet.SharedViewModels.Models;
using HongPet.SharedViewModels.ViewModels;

namespace HongPet.CustomerMVC.Services.Abstraction;

public interface IOrderApiService
{
    Task<Guid> CreateOrderAsync(OrderCreationModel orderModel);
    Task<OrderVM> GetOrderByIdAsync(Guid orderId);
    Task<IPagedList<OrderVM>> GetUserOrdersAsync(Guid userId,
        QueryListCriteria criteria);
}
