using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using HongPet.CustomerMVC.Models;
using HongPet.CustomerMVC.Services.Abstraction;
using HongPet.SharedViewModels.Models;
using Microsoft.AspNetCore.Mvc;

namespace HongPet.CustomerMVC.Controllers;

public class HomeController(
    ILogger<HomeController> _logger,
    IProductApiService _productApiService) : Controller
{

    public async Task<IActionResult> Index()
    {
        try
        {
            var products = (await _productApiService
            .GetProductsAsync(new QueryListCriteria
            {
                PageIndex = 1,
                PageSize = 3
            })).Items;

            return View(products);
        } catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching feature products.");

            return RedirectToAction(nameof(Error), new { errMsg = ex.Message });
        }
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    
    public IActionResult Error(string? errMsg)
    {
        ViewBag.ErrorMessage = errMsg;

        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    
}
