using HongPet.CustomerMVC.Models;
using HongPet.CustomerMVC.Services.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace HongPet.CustomerMVC.Controllers;
public class CartController(
    ILogger<CartController> _logger,
    ICartService _cartService) : Controller
{
    public IActionResult Index()
    {
        var cart = _cartService.GetCartItems();
        return View(cart);
    }

    [HttpPost]
    public IActionResult AddToCart(CartItemModel model)
    {
        try
        {
            _cartService.AddToCart(model);
            TempData["SuccessMessage"] = "Thêm sản phẩm vào giỏ hàng thành công!";             
        } catch (Exception ex)
        {
            TempData["ErrorMessage"] = "Thêm sản phẩm vào giỏ hàng thất bại: " + ex.Message;            
        }
        return RedirectToAction("Detail", "Product", new { id = model.ProductId });
    }

    [HttpGet]
    public IActionResult GetCartItemsQuantity()
    {
        try
        {
            var quantity = _cartService.GetCartItemsQuantity();
            return Json(quantity);
        } catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    public IActionResult DecreaseQuantity(Guid variantId)
    {
        try
        {
            _cartService.IncreaseQuantity(variantId, -1);            
        } catch (Exception ex)
        {
            _logger.LogError(ex, $"Decrease quantity failed: {ex.Message}");            
        }
        return RedirectToAction("Index");
    }

    public IActionResult IncreaseQuantity(Guid variantId)
    {
        try
        {
            _cartService.IncreaseQuantity(variantId, 1);
        } catch (Exception ex)
        {
            _logger.LogError(ex, $"Increase quantity failed: {ex.Message}");
        }
        return RedirectToAction("Index");
    }

    public IActionResult RemoveFromCart(Guid variantId)
    {
        try
        {
            _cartService.RemoveFromCart(variantId);            
        } catch (Exception ex)
        {
            _logger.LogError(ex, $"Remove item from cart failed: {ex.Message}");
        }
        return RedirectToAction("Index");
    }


}
