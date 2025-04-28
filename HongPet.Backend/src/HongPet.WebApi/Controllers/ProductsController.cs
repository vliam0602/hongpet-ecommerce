using HongPet.Application.Services.Abstractions;
using HongPet.SharedViewModels.Generals;
using HongPet.SharedViewModels.Models;
using HongPet.SharedViewModels.ViewModels;
using Microsoft.AspNetCore.Authorization;
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
    public async Task<ActionResult<PagedList<ProductGeneralVM>>> GetProducts(
        [FromQuery] QueryListCriteria criteria)
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
            var productVM = await _productService.GetProductDetailAsync(id);
            return Ok(new ApiResponse
            {
                IsSuccess = true,
                Data = productVM
            });
        } catch (KeyNotFoundException)
        {
            return NotFound(new ApiResponse
            {
                IsSuccess = false,
                ErrorMessage = "Product is not exist!"
            });
        } catch (Exception ex)
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
    public async Task<ActionResult<PagedList<ReviewVM>>> GetProductReviews(Guid productId, 
        [FromQuery] QueryListCriteria criteria)
    {
        try
        {
            var reviews = await _reviewService.GetReviewsByProductIdAsync(productId, 
                criteria.PageIndex, criteria.PageSize);
            return Ok(new ApiResponse
            {
                IsSuccess = true,
                Data = reviews
            });
        } catch (Exception ex)
        {
            _logger.LogError(ex, $"**Unexpected error** Error occurred while getting product with id {productId}.");
            return StatusCode(500, new ApiResponse
            {
                IsSuccess = false,
                ErrorMessage = $"Unexpected error: Error occurred while getting product with id {productId}. Details: {ex.Message}"
            });
        }
    }

    [HttpPost("/admin/api/products")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddProduct([FromBody] ProductModel productModel)
    {
        try
        {
            var productVM = await _productService.AddProductAsync(productModel);
            return CreatedAtAction(nameof(AddProduct), new {Id = productVM.Id},new ApiResponse
            {
                Message = "Product added successfully.",
                Data = productVM
            });
        } catch (Exception ex)
        {
            _logger.LogError(ex, "**Unexpected error** Error occurred while adding a new product.");
            return StatusCode(500, new ApiResponse
            {
                IsSuccess = false,
                ErrorMessage = $"Unexpected error: Error occurred while adding a new product. Details: {ex.Message}"
            });
        }
    }

    [HttpPut("/admin/api/products/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateProduct(Guid id, 
        [FromBody] ProductModel productModel)
    {
        try
        {
            var productVM = await _productService.UpdateProductAsync(id, productModel);
            return Ok(new ApiResponse
            {
                Message = $"Product {id} updated successfully.",
                Data = productVM
            });
        } catch (KeyNotFoundException ex)
        {
            return NotFound(new ApiResponse
            {
                IsSuccess = false,
                ErrorMessage = ex.Message
            });
        } catch (Exception ex)
        {
            _logger.LogError(ex, $"**Unexpected error** Error occurred while updating product with id {id}.");
            return StatusCode(500, new ApiResponse
            {
                IsSuccess = false,
                ErrorMessage = $"Unexpected error: Error occurred while updating product with id {id}. Details: {ex.Message}"
            });
        }
    }

    [HttpDelete("/admin/api/products/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> SoftDeleteProduct(Guid id)
    {
        try
        {
            await _productService.SoftDeleteProductAsync(id);
            return Ok(new ApiResponse
            {
                Message = $"Product {id} deleted successfully."
            });
        } catch (KeyNotFoundException ex)
        {
            return NotFound(new ApiResponse
            {
                IsSuccess = false,
                ErrorMessage = ex.Message
            });
        } catch (Exception ex)
        {
            _logger.LogError(ex, $"**Unexpected error** Error occurred while deleting product with id {id}.");
            return StatusCode(500, new ApiResponse
            {
                IsSuccess = false,
                ErrorMessage = $"Unexpected error: Error occurred while deleting product with id {id}. Details: {ex.Message}"
            });
        }
    }
}
