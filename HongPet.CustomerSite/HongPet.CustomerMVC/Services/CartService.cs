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

    public void AddToCart(Guid variantId, int quantity)
    {
        var cartItems = GetCartItems().ToList();

        var existingItem = cartItems
            .FirstOrDefault(item => item.VariantId == variantId);

        if (existingItem != null)
        {
            existingItem.Quantity += quantity;
        } else
        {
            cartItems.Add(new CartItemModel
            {
                VariantId = variantId,
                Quantity = quantity
            });
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
}

