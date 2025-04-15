using Domain.Entities.Commons;

namespace HongPet.Domain.Entities;

public class ProductAttribute : BaseEntity
{
    public string Name { get; set; } = default!;

    public virtual IEnumerable<ProductAttributeValue> AttributeValues { get; set; } = new List<ProductAttributeValue>();
}
