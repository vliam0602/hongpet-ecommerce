using HongPet.Domain.Entities.Commons;

namespace HongPet.Domain.Entities;

public class ProductAttributeValue : BaseEntity
{
    public Guid AttributeId { get; set; }
    public string Value { get; set; } = default!;    

    public virtual ProductAttribute Attribute { get; set; } = default!;
    public virtual IEnumerable<Variant> Variants { get; set; } = new List<Variant>();
}
