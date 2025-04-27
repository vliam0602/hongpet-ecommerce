using HongPet.CustomerMVC.Models;
using HongPet.CustomerMVC.Services.Abstraction;
using HongPet.SharedViewModels.Models;
using Microsoft.AspNetCore.Mvc;

namespace HongPet.CustomerMVC.Controllers;
public class ProductController(
    IProductApiService _productApiService) : Controller
{
    public async Task<IActionResult> Index(int pageIndex = 1, int pageSize = 6, 
        string searchString = "")
    {
        ViewData["searchString"] = searchString;

        var products = (await _productApiService
            .GetProductsAsync(new QueryListCriteria
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                Keyword = searchString
            }));

        return View(new ProductListViewModel 
        { 
            ProductPagedList = products,
            SearchString = searchString
        });       
    }

    public async Task<IActionResult> Detail(Guid id)
    {
        var product = await _productApiService.GetProductByIdAsync(id);
        if (product == null)
        {
            return RedirectToAction("NotFoundError", "Home");
        }
        return View(product);
    }

}
