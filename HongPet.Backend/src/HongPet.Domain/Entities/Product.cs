using HongPet.Domain.Entities.Commons;

namespace HongPet.Domain.Entities;

public class Product : BaseEntity
{
    public string Name { get; set; } = default!;    
    public string? Description { get; set; }
    public string? Brief { get; set; }
    public decimal Price { get; set; } = 0; // product price is min price of variants
    public string? ThumbnailUrl { get; set; }
    public bool IsActive { get; set; } = true;

    public virtual IEnumerable<Category> Categories { get; set; } = new List<Category>();
    public virtual IEnumerable<Variant> Variants { get; set; } = new List<Variant>();
    public virtual IEnumerable<Image> Images { get; set; } = new List<Image>();
    public virtual IEnumerable<Review> Reviews { get; set; } = new List<Review>();
}
