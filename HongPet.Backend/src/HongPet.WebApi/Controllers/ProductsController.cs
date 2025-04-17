using Domain.Entities.Commons;
using HongPet.Application.Commons;
using HongPet.Domain.Entities;
using HongPet.SharedViewModels.Response;
using HongPet.SharedViewModels.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HongPet.WebApi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly ILogger<ProductsController> _logger;
    private readonly IGenericService<Product, ProductVM> _productService;
    public ProductsController(ILogger<ProductsController> logger,
        IGenericService<Product, ProductVM> productService)
    {
        _logger = logger;
        _productService = productService;
    }
    [HttpGet]
    public async Task<ActionResult<PagedList<ProductVM>>> GetProducts([FromQuery] int pageIndex,int pageSize)
    {
        var products = await _productService.GetPagedAsync(pageIndex, pageSize);        
        return Ok(new ApiResponse
        {
            IsSuccess = true,
            Data = products
        });
    }
}
