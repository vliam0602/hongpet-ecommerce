using HongPet.Application.Commons;
using HongPet.Application.Services.Abstractions;
using HongPet.SharedViewModels.Models;
using HongPet.SharedViewModels.Response;
using HongPet.SharedViewModels.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HongPet.WebApi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ProductsController(
    ILogger<ProductsController> _logger,
    IProductService _productService) : ControllerBase
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
            _logger.LogError(ex, "Error occurred while getting products list.");
            return BadRequest(new ApiResponse
            {
                IsSuccess = false,
                ErrorMessage = ex.Message
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
                ErrorMessage = "Sản phẩm không tồn tại"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"*** UNHANDLED EXCEPTION *** Error occurred while getting product with id {id}.");
            return BadRequest(new ApiResponse
            {
                IsSuccess = false,
                ErrorMessage = ex.Message
            });
        }
    }

    [HttpGet("{productId}/reviews")]
    public async Task<ActionResult<PagedList<ReviewVM>>> GetProductReviews(Guid productId, [FromQuery] QueryListCriteria criteria)
    {
        try
        {
            var reviews = await _productService.GetProductReviewsAsync(productId, criteria.PageIndex, criteria.PageSize);
            return Ok(new ApiResponse
            {
                IsSuccess = true,
                Data = reviews
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"*** UNHANDLED EXCEPTION *** Error occurred while getting product reviews with productId {productId}.");
            return BadRequest(new ApiResponse
            {
                IsSuccess = false,
                ErrorMessage = ex.Message
            });
        }
    }
}
