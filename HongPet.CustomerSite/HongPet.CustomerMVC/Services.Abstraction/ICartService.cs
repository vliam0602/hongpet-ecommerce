using HongPet.CustomerMVC.Models;

namespace HongPet.CustomerMVC.Services.Abstraction;

public interface ICartService
{
    void AddToCart(Guid variantId, int quantity);
    IEnumerable<CartItemModel> GetCartItems();
    void ClearCart();
}
