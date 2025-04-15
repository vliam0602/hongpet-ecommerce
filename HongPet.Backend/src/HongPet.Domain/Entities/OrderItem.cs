using Domain.Entities.Commons;

namespace HongPet.Domain.Entities;

public class OrderItem
{
    public Guid OrderId { get; set; }
    public Guid VariantId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }

    public virtual Order Order { get; set; } = default!;
    public virtual Variant Variant { get; set; } = default!;
}
