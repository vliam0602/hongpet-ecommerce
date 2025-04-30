using HongPet.CustomerMVC.Models;
using HongPet.CustomerMVC.Services;
using HongPet.CustomerMVC.Services.Abstraction;
using HongPet.SharedViewModels.Models;
using Microsoft.AspNetCore.Mvc;

public class OrderController(
    IOrderApiService _orderApiService,
    ICartService _cartService,
    IClaimService _claimService) : Controller
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

            var orderIdCreated = await _orderApiService.CreateOrderAsync(orderModel);

            // Clear the cart after make order successfully
            _cartService.ClearCart();

            return RedirectToAction(nameof(OrderConfirm),
                new { orderId = orderIdCreated });

        } catch (Exception ex)
        {
            return RedirectToAction("Error", "Home", new { errMsg = ex.Message });
        }
    }
    
    //TODO: add catch unauthorized filter
    public async Task<IActionResult> OrderHistory(
        int pageIndex = 1, int pageSize = 5)
    {
        var orders = await _orderApiService
            .GetUserOrdersAsync(_claimService.UserId!.Value, new QueryListCriteria
            {
                PageIndex = pageIndex,
                PageSize = pageSize
            });            
        return View(new OrderListViewModel
        {
            OrderPagedList = orders
        });
    }
}
