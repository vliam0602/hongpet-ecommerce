using HongPet.CustomerMVC.Models;
using HongPet.CustomerMVC.Services.Abstraction;
using HongPet.CustomerMVC.Utilities;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace HongPet.CustomerMVC.Services;

public class CartService(
    IHttpContextAccessor httpContextAccessor) : ICartService
{
    private readonly ISession _session = httpContextAccessor.HttpContext!.Session;

    public void AddToCart(CartItemModel model)
    {
        var cartItems = GetCartItems().ToList();

        var existingItem = cartItems
            .FirstOrDefault(item => item.VariantId == model.VariantId);

        if (existingItem != null)
        {
            existingItem.Quantity += model.Quantity;
        } else
        {
            cartItems.Add(model);
        }

        SaveCartItems(cartItems);
    }

    public IEnumerable<CartItemModel> GetCartItems()
    {
        var cartData = _session.GetString(AppConstant.CartSessionKey);

        return string.IsNullOrEmpty(cartData)
            ? new List<CartItemModel>()
            : JsonConvert.DeserializeObject<List<CartItemModel>>(cartData)!;
    }

    public void ClearCart()
    {
        _session.Remove(AppConstant.CartSessionKey);
    }

    private void SaveCartItems(IEnumerable<CartItemModel> cartItems)
    {
        var cartData = JsonConvert.SerializeObject(cartItems);
        _session.SetString(AppConstant.CartSessionKey, cartData);
    }

    public int GetCartItemsQuantity()
    {
        var cartData = GetCartItems();

        return cartData == null || !cartData.Any()
            ? 0
            : cartData.Sum(x => x.Quantity);
    }

    public void IncreaseQuantity(Guid variantId, int quantity)
    {
        var cartItems = GetCartItems().ToList();

        var existingItem = cartItems
            .FirstOrDefault(item => item.VariantId == variantId);

        if (existingItem != null)
        {
            if (quantity > 0 || existingItem.Quantity > 1)
            {
                existingItem.Quantity += quantity;
            } else
            {
                cartItems.Remove(existingItem);
            }
            SaveCartItems(cartItems);
        }
    }

    public void RemoveFromCart(Guid variantId)
    {
        var cartItems = GetCartItems().ToList();

        var itemToRemove = cartItems
            .FirstOrDefault(item => item.VariantId == variantId);

        if (itemToRemove != null)
        {
            cartItems.Remove(itemToRemove);
            SaveCartItems(cartItems);
        }
    }
}

