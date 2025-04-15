﻿using Domain.Entities.Commons;
using HongPet.Domain.Enums;

namespace HongPet.Domain.Entities;

public class Order : BaseEntity
{
    public Guid CustomerId { get; set; }
    public string CustomerName { get; set; } = default!;
    public string CustomerEmail { get; set; } = default!;
    public string CustomerPhone { get; set; } = default!;
    public string ShippingAddress { get; set; } = default!;
    public decimal TotalAmount { get; set; }
    public OrderStatusEnum Status { get; set; } = OrderStatusEnum.Pending;

    public virtual User Customer { get; set; } = default!;
    public virtual IEnumerable<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    public virtual IEnumerable<Review> Reviews { get; set; } = new List<Review>();
}
