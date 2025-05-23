﻿using HongPet.Domain.Entities.Commons;

namespace HongPet.Domain.Entities;

public class Variant : BaseEntity
{
    public Guid ProductId { get; set; }
    public string? ProductName { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public bool IsActive { get; set; } = true;       

    public virtual Product Product { get; set; } = default!;
    public virtual ICollection<ProductAttributeValue> AttributeValues { get; set; } = new List<ProductAttributeValue>();
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
