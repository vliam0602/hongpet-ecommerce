using HongPet.Application.Commons;
using HongPet.Application.Services.Abstractions;
using HongPet.SharedViewModels.Response;
using HongPet.SharedViewModels.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HongPet.WebApi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly ILogger<ProductsController> _logger;
    private readonly IProductService _productService;
    public ProductsController(ILogger<ProductsController> logger,
        IProductService productService)
    {
        _logger = logger;
        _productService = productService;
    }
    [HttpGet]
    [Authorize]
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
