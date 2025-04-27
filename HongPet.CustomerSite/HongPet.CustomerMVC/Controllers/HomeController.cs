using System.Diagnostics;
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
        // Dữ liệu mẫu
        //var products = new List<ProductGeneralVM>
        //    {
        //        new ProductGeneralVM { 
        //            Name = "XXXXX", 
        //            Price = 999, 
        //            ThumbnailUrl = "https://i.ebayimg.com/thumbs/images/g/VQgAAOSwvuZiF~ai/s-l225.jpg" 
        //        },
        //        new ProductGeneralVM {
        //            Name = "XXXX", 
        //            Price = 999, 
        //            ThumbnailUrl = "https://i.ebayimg.com/thumbs/images/g/VQgAAOSwvuZiF~ai/s-l225.jpg" 
        //        },
        //        new ProductGeneralVM { 
        //            Name = "XXX", 
        //            Price = 999, 
        //            ThumbnailUrl = "https://i.ebayimg.com/thumbs/images/g/VQgAAOSwvuZiF~ai/s-l225.jpg" 
        //        }
        //    };
        var products = (await _productApiService
            .GetProductsAsync(new QueryListCriteria
            {
                PageIndex = 1,
                PageSize = 3
            })).Items;

        // Truyền model vào view
        return View(products);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
