using HongPet.CustomerMVC.Services;
using HongPet.CustomerMVC.Services.Abstraction;
using HongPet.SharedViewModels.Models;
using Microsoft.AspNetCore.Mvc;

public class OrderController(
    IOrderApiService _orderApiService,
    ICartService _cartService) : Controller
{
    public IActionResult Index()
    {
        var cart = _cartService.GetCartItems();
        return View(cart);
    }

    public async Task<IActionResult> OrderConfirm(Guid orderId)
    {
        try
        {
            var order = await _orderApiService.GetOrderByIdAsync(orderId);
            return View(order);
        } catch (KeyNotFoundException)
        {
            return RedirectToAction("NotFoundError", "Home");
        } catch (Exception ex)
        {
            return RedirectToAction("Error", "Home", new { errMsg = ex.Message });
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> MakeOrder(OrderCreationModel orderModel)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View("Index", orderModel);
            }

            var orderCreated = await _orderApiService.CreateOrderAsync(orderModel);

            // Clear the cart after make order successfully
            _cartService.ClearCart();

            return RedirectToAction(nameof(OrderConfirm),
                new { orderId = orderCreated.Id });

        } catch (Exception ex)
        {
            return RedirectToAction("Error", "Home", new { errMsg = ex.Message });
        }
    }
}
