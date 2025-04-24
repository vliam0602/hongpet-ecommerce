using HongPet.Application.Commons;
using HongPet.Application.Services.Abstractions;
using HongPet.SharedViewModels.Models;
using HongPet.SharedViewModels.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HongPet.WebApi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ProductsController(
    ILogger<ProductsController> _logger,
    IProductService _productService,
    IReviewService _reviewService) : ControllerBase
{

    [HttpGet]
    public async Task<ActionResult<PagedList<ProductGeneralVM>>> GetProducts([FromQuery] QueryListCriteria criteria)
    {
        try
        {
            var products = await _productService.GetPagedProductAsync(criteria.PageIndex, criteria.PageSize, criteria.Keyword);

            return Ok(new ApiResponse
            {
                IsSuccess = true,
                Data = products
            });
        } catch (Exception ex)
        {
            _logger.LogError(ex, "**Unexpected error** Error occurred while getting products list.");
            return StatusCode(500, new ApiResponse
            {
                IsSuccess = false,
                ErrorMessage = $"Unexpected error: Error occurred while getting products list. Details: {ex.Message}"
            });
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductGeneralVM>> GetProduct(Guid id)
    {
        try
        {
            var product = await _productService.GetProductDetailAsync(id);           
            return Ok(new ApiResponse
            {
                IsSuccess = true,
                Data = product
            });
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new ApiResponse
            {
                IsSuccess = false,
                ErrorMessage = "Product is not exist!"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"**Unexpected error** Error occurred while getting product with id {id}.");
            return StatusCode(500, new ApiResponse
            {
                IsSuccess = false,
                ErrorMessage = $"Unexpected error:  Error occurred while getting product with id {id}. Details: {ex.Message}"
            });
        }
    }

    [HttpGet("{productId}/reviews")]
    public async Task<ActionResult<PagedList<ReviewVM>>> GetProductReviews(Guid productId, [FromQuery] QueryListCriteria criteria)
    {
        try
        {
            var reviews = await _reviewService.GetReviewsByProductIdAsync(productId, criteria.PageIndex, criteria.PageSize);
            return Ok(new ApiResponse
            {
                IsSuccess = true,
                Data = reviews
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"**Unexpected error** Error occurred while getting product with id {productId}.");
            return StatusCode(500, new ApiResponse
            {
                IsSuccess = false,
                ErrorMessage = $"Unexpected error: Error occurred while getting product with id {productId}. Details: {ex.Message}"
            });
        }
    }
}
