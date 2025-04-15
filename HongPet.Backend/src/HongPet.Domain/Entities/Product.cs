using Domain.Entities.Commons;

namespace HongPet.Domain.Entities;

public class Product : BaseEntity
{
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
   
    public virtual IEnumerable<Category> Categories { get; set; } = new List<Category>();
    public virtual IEnumerable<Variant> Variants { get; set; } = new List<Variant>();
    public virtual IEnumerable<Image> Images { get; set; } = new List<Image>();
    public virtual IEnumerable<Review> Reviews { get; set; } = new List<Review>();
}
