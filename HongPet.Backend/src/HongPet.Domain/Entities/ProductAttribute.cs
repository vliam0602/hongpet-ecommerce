using HongPet.Domain.Entities.Commons;

namespace HongPet.Domain.Entities;

public class ProductAttribute : BaseEntity
{
    public string Name { get; set; } = default!;

    public virtual ICollection<ProductAttributeValue> AttributeValues { get; set; } = new List<ProductAttributeValue>();
}
