using AutoMapper;
using HongPet.Application.Services.Abstractions;
using HongPet.Domain.Entities;
using HongPet.SharedViewModels.Generals;
using HongPet.SharedViewModels.Models;
using HongPet.SharedViewModels.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HongPet.WebApi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class CategoriesController(
    ILogger<CategoriesController> _logger,
    ICategoryService _categoryService,
    IMapper _mapper) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PagedList<CategoryVM>>> GetCategories(
        [FromQuery] QueryListCriteria criteria)
    {
        try
        {
            var categories = await _categoryService.GetPagedCategoriesAsync(
                criteria.PageIndex, criteria.PageSize, criteria.Keyword);
            return Ok(new ApiResponse
            {
                Data = categories
            });
        } catch (Exception ex)
        {
            _logger.LogError(ex,
                $"Unexpected error: Error occurred while getting categoris list.");
            return StatusCode(500, new ApiResponse
            {
                IsSuccess = false,
                ErrorMessage = $"Unexpected error: Error occurred while getting categories list. " +
                $"Details: {ex.Message}"
            });
        }
    }

    [HttpGet("all")]
    public async Task<ActionResult<CategoryVM>> GetAllCategories()
    {
        try
        {
            var categories = await _categoryService.GetAllAsync();
            return Ok(new ApiResponse
            {
                Data = categories
            });
        } catch (Exception ex)
        {
            _logger.LogError(ex,
                $"Unexpected error: Error occurred while getting categoris list.");
            return StatusCode(500, new ApiResponse
            {
                IsSuccess = false,
                ErrorMessage = $"Unexpected error: Error occurred while getting categories list. " +
                $"Details: {ex.Message}"
            });
        }
    }
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateCategory(
        [FromBody] CategoryCreateModel categoryModel)
    {
        try
        {
            var category = _mapper.Map<Category>(categoryModel);
            var createdId = await _categoryService
                .AddAsync(category);
            return CreatedAtAction(nameof(GetCategories),
                new { id = createdId }, new ApiResponse
                {
                    Message = $"Create category {categoryModel.Name} sucessfull.",
                    Data = createdId
                });
        } catch (Exception ex)
        {
            _logger.LogError(ex,
                $"Unexpected error: Error occurred while creating category.");
            return StatusCode(500, new ApiResponse
            {
                IsSuccess = false,
                ErrorMessage = $"Unexpected error: Error occurred while creating category. " +
                $"Details: {ex.Message}"
            });
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CategoryVM>> GetCategoryDetail(Guid id)
    {
        try
        {
            var category = await _categoryService.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound(new ApiResponse
                {
                    IsSuccess = false,
                    ErrorMessage = $"Category with id {id} not found."
                });
            }
            return Ok(new ApiResponse
            {
                Data = _mapper.Map<CategoryVM>(category)
            });
        } catch (Exception ex)
        {
            _logger.LogError(ex,
                $"Unexpected error: Error occurred while getting category detail.");
            return StatusCode(500, new ApiResponse
            {
                IsSuccess = false,
                ErrorMessage = $"Unexpected error: Error occurred while getting category detail. " +
                $"Details: {ex.Message}"
            });
        }
    }
    
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateCategory(Guid id, [FromBody] CategoryCreateModel categoryModel)
    {
        try
        {
            var updatedCategory = await _categoryService.UpdateCategoryAsync(id, categoryModel);
            return Ok(new ApiResponse
            {
                Message = $"Category {id} updated successfully.",
                Data = updatedCategory
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
            _logger.LogError(ex, $"**Unexpected error** Error occurred while updating category with id {id}.");
            return StatusCode(500, new ApiResponse
            {
                IsSuccess = false,
                ErrorMessage = $"Unexpected error: Error occurred while updating category with id {id}. Details: {ex.Message}"
            });
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")] // hard delete
    public async Task<IActionResult> DeleteCategory(Guid id)
    {
        try
        {
            await _categoryService.DeleteAsync(id);
            return Ok(new ApiResponse
            {
                Message = $"Category {id} deleted successfully."
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
            _logger.LogError(ex, $"**Unexpected error** Error occurred while deleting category with id {id}.");
            return StatusCode(500, new ApiResponse
            {
                IsSuccess = false,
                ErrorMessage = $"Unexpected error: Error occurred while deleting category with id {id}. Details: {ex.Message}"
            });
        }
    }


}
