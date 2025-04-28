using HongPet.CustomerMVC.Models;
using HongPet.CustomerMVC.Services.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace HongPet.CustomerMVC.Controllers;
public class CartController(
    ICartService _cartService) : Controller
{
    public IActionResult Index()
    {
        var cart = _cartService.GetCartItems();
        return View(cart);
    }

    [HttpPost]
    public IActionResult AddToCart([FromBody] CartItemModel model)
    {
        try
        {
            _cartService.AddToCart(model.VariantId, model.Quantity);
            return Ok();
        } catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    
}
