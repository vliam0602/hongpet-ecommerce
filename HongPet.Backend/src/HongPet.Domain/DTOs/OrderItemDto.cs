using HongPet.Domain.Entities;
using HongPet.SharedViewModels.ViewModels;

namespace HongPet.Infrastructure.DTOs;
public class OrderItemDto
{
    public Guid ProductId { get; set; }
    public Guid VariantId { get; set; }
    public string? ProductName { get; set; }
    public string? ThumbnailImageUrl { get; set; }
    public IEnumerable<AttributeValueDto> AttributeValues { get; set; }
        = new List<AttributeValueDto>();
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}
