using HongPet.Domain.Entities.Commons;

namespace HongPet.Domain.Entities;

public class Review : BaseEntity
{
    public Guid OrderId { get; set; }
    public Guid CustomerId { get; set; }
    public Guid ProductId { get; set; }
    public int Rating { get; set; }
    public string Title { get; set; } = default!;
    public string Comment { get; set; } = default!;

    public virtual Order Order { get; set; } = default!;
    public virtual User Customer { get; set; } = default!;
    public virtual Product Product { get; set; } = default!;
}
