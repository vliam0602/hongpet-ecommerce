using HongPet.CustomerMVC.Models;

namespace HongPet.CustomerMVC.Services.Abstraction;

public interface ICartService
{
    void AddToCart(CartItemModel model);
    IEnumerable<CartItemModel> GetCartItems();
    int GetCartItemsQuantity();
    void ClearCart();
}
