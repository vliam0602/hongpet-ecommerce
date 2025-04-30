namespace HongPet.Infrastructure.DTOs;
public class AttributeValueDto
{
    public Guid Id { get; set; }
    public string Attribute { get; set; } = default!;
    public string Value { get; set; } = default!;
}
