namespace HongPet.CustomerMVC.Models;

public class CartItemModel
{
    public Guid ProductId { get; set; }
    public Guid VariantId { get; set; }
    public string ProductName { get; set; } = default!;
    public string VariantName { get; set; } = default!;
    public string ThumbnailImageUrl { get; set; } = default!;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public decimal Subtotal => Price * Quantity;
}

