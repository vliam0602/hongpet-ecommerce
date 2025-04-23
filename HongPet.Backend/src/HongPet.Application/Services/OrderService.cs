using AutoMapper;
using Domain.Entities.Commons;
using HongPet.Application.Commons;
using HongPet.Application.Services.Abstractions;
using HongPet.Application.Services.Commons;
using HongPet.Domain.Entities;
using HongPet.Domain.Enums;
using HongPet.Domain.Repositories.Abstractions;
using HongPet.SharedViewModels.Models;
using HongPet.SharedViewModels.ViewModels;

namespace HongPet.Application.Services;
public class OrderService : GenericService<Order>, IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IGenericRepository<Variant> _variantRepository;
    private readonly IMapper _mapper;
    private readonly IClaimService _claimService;
    public OrderService(
        IUnitOfWork unitOfWork, 
        IMapper mapper,
        IClaimService claimService) : base(unitOfWork)
    {
        _orderRepository = unitOfWork.OrderRepository;
        _repository = _orderRepository; // for reuse the generic service methods
        _mapper = mapper;
        _variantRepository = _unitOfWork.Repository<Variant>();
        _claimService = claimService;
    }

    public async Task<IPagedList<OrderVM>> GetOrdersByCustomerIdAsync(
        Guid customerId, int? pageIndex = 1, int? pageSize = 10, string? searchKey = "")
    {
        var currentUserId = _claimService.GetCurrentUserId;
        var isAdmin = _claimService.IsAdmin;
        if (currentUserId == null || isAdmin == null)
        {
            throw new UnauthorizedAccessException("Authorization info not found");
        }
        if (isAdmin == false && currentUserId != customerId)
        {
            throw new UnauthorizedAccessException("Bạn không có quyền truy cập thông tin này.");
        }

        var orders = await _orderRepository.GetOrderByCustomerIdAsync(
            customerId, pageIndex ?? 1, pageSize ?? 10, searchKey ?? "");
        return _mapper.Map<PagedList<OrderVM>>(orders);

    }

    public Task<OrderVM?> GetOrderWithDetailsAsync(Guid orderId)
    {
        throw new NotImplementedException();
    }

    public async Task<Order> CreateOrderAsync(OrderCreationModel orderModel)
    {
        // validate the string enum
        if (!Enum.IsDefined(typeof(OrderStatusEnum), orderModel.Status)) {
            throw new ArgumentException(
                $"Invalid order status. Order status should be in range " +
                $"[{string.Join(", ", Enum.GetValues<OrderStatusEnum>())}]");
        }
        if (!Enum.IsDefined(typeof(PaymentMethodEnum), orderModel.PaymentMethod)) {
            throw new ArgumentException(
                $"Invalid payment method. Payment method should be in range " +
                $"[{string.Join(", ", Enum.GetValues<PaymentMethodEnum>())}]");
        }
        var customerId = _claimService.GetCurrentUserId;
        if (customerId == null)
        {
            throw new ArgumentException("Customer id is not found.");
        }

        var order = _mapper.Map<Order>(orderModel);

        order.CustomerId = customerId.Value;

        // validate the variantId
        foreach (var item in order.OrderItems)
        {
            var variant = await _variantRepository.GetByIdAsync(item.VariantId);
            if (variant == null)
            {
                throw new ArgumentException(
                    $"Variant product with id {item.VariantId} not found.");
            }
            item.Variant = variant;
            item.OrderId = order.Id;
        }

        // add the order to the db
        await _orderRepository.AddAsync(order);
        if (await _unitOfWork.SaveChangesAsync() == 0)
        {
            throw new Exception("Failed to save new order in db.");
        }

        return order;
    }

    public Task<bool> UpdateOrderStatusAsync(Guid orderId, OrderStatusEnum status)
    {
        throw new NotImplementedException();
    }
}
