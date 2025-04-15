using Domain.Entities.Commons;

namespace HongPet.Domain.Entities;

public class Category : BaseEntity
{    
    public string Name { get; set; } = default!;
    public Guid? ParrentCategoryId { get; set; }

    public virtual Category? ParentCategory { get; set; }
    public virtual IEnumerable<Category> SubCategories { get; set; } = new List<Category>();
    public virtual IEnumerable<Product> Products { get; set; } = new List<Product>();
}
