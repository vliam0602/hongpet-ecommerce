using HongPet.CustomerMVC.Models;

namespace HongPet.CustomerMVC.Services.Abstraction;

public interface ICartService
{
    void AddToCart(CartItemModel model);
    void IncreaseQuantity(Guid variantId, int quantity);    
    void RemoveFromCart(Guid variantId);    
    IEnumerable<CartItemModel> GetCartItems();
    int GetCartItemsQuantity();
    void ClearCart();
}
