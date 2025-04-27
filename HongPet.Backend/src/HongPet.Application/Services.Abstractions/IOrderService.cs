using HongPet.Application.Commons;
using HongPet.Domain.Entities;
using HongPet.Domain.Enums;
using HongPet.SharedViewModels.Generals;
using HongPet.SharedViewModels.Models;
using HongPet.SharedViewModels.ViewModels;

namespace HongPet.Application.Services.Abstractions;
public interface IOrderService : IGenericService<Order>
{
    Task<Order> CreateOrderAsync(OrderCreationModel orderModel);
    Task<IPagedList<OrderVM>> GetOrdersByCustomerIdAsync(
        Guid customerId, int? pageIndex = 1, int? pageSize = 10, string? seachKey = "");
    Task<OrderVM?> GetOrderWithDetailsAsync(Guid orderId);
    Task<bool> UpdateOrderStatusAsync(Guid orderId, OrderStatusEnum status);    
}
