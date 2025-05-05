using HongPet.CustomerMVC.Models;
using HongPet.CustomerMVC.Services.Abstraction;
using HongPet.SharedViewModels.Models;
using Microsoft.AspNetCore.Mvc;

namespace HongPet.CustomerMVC.Controllers;
public class ProductController(
    IProductApiService _productApiService) : Controller
{
    public async Task<IActionResult> Index(int pageIndex = 1, int pageSize = 6, 
        string searchString = "", List<string>? categories = null)
    {
        try
        {
            ViewData["searchString"] = searchString;
            
            var categoriesVM = await _productApiService.GetAllCategoriesAsync();

            var products = await _productApiService
                .GetProductsAsync(new QueryListCriteria
                {
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    Keyword = searchString
                }, categories);

            return View(new ProductListViewModel
            {
                ProductPagedList = products,
                SearchString = searchString,
                Categories = categoriesVM
            });
        } catch (Exception ex)
        {
            return RedirectToAction("Error", "Home", new { errMsg = ex.Message });
        }
    }

    public async Task<ActionResult<ProductDetailViewModel>> Detail(Guid id,
        int reviewPageIndex = 1, int reviewPageSize = 3)
    {
        try
        {
            var product = await _productApiService.GetProductByIdAsync(id);
            var viewModel = new ProductDetailViewModel
            {
                ProductDetail = product,
                Reviews = await _productApiService.GetProductReviewsAsync(
                    id, reviewPageIndex, reviewPageSize)
            };
            return View(viewModel);
        } catch (KeyNotFoundException)
        {
            return RedirectToAction("NotFoundError", "Home");
        } catch (HttpRequestException ex)
        {
            return RedirectToAction("Error", "Home", new { errMsg = ex.Message });
        }
    }


}
