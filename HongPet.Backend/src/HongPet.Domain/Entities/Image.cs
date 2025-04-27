using HongPet.Domain.Entities.Commons;

namespace HongPet.Domain.Entities;

public class Image : BaseEntity
{
    public Guid ProductId { get; set; }
    public string Name { get; set; } = default!;
    public string ImageUrl { get; set; } = default!;           

    public virtual Product Product { get; set; } = default!;
}
