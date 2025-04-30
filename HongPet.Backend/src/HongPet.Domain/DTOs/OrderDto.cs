using HongPet.Domain.Enums;

namespace HongPet.Infrastructure.DTOs;
public class OrderDto
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public string CustomerName { get; set; } = default!;
    public string CustomerEmail { get; set; } = default!;
    public string CustomerPhone { get; set; } = default!;
    public string ShippingAddress { get; set; } = default!;
    public decimal TotalAmount { get; set; }
    public OrderStatusEnum Status { get; set; } = OrderStatusEnum.Pending;
    public PaymentMethodEnum PaymentMethod { get; set; } = PaymentMethodEnum.COD;

    public virtual IEnumerable<OrderItemDto> OrderItems { get; set; } 
        = new List<OrderItemDto>();
    public DateTime CreatedDate { get; set; }
}
