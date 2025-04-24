using AutoMapper;
using HongPet.Application.Services.Abstractions;
using HongPet.Domain.Entities;
using HongPet.SharedViewModels.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HongPet.WebApi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ReviewsController(
    IReviewService _reviewService,
    IMapper _mapper) : ControllerBase
{
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateReview(
        [FromBody] ReviewCreateModel model)
    {
        try
        {
            var review = await _reviewService.CreateReviewAsync(model);
            return CreatedAtAction("CreateReview", new { id = review.Id }, 
                new ApiResponse
                {
                    Data =  review
                });

        } 
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new ApiResponse
            {
                IsSuccess = false,
                ErrorMessage = ex.Message
            });
        }
        catch(KeyNotFoundException ex)
        {
            return NotFound(new ApiResponse
            {
                IsSuccess = false,
                ErrorMessage = ex.Message
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse
            {
                IsSuccess = false,
                ErrorMessage =
                $"**Unexpected Error**: An error occurs when create new review." +
                $"Details {ex.Message}"
            });
        }
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> UpdateReview(Guid id,
        [FromBody] ReviewUpdateModel model)
    {
        try
        {            
            var reviewVM = await _reviewService.UpdateReviewAsync(id, model);

            return Ok(new ApiResponse
            {
                Message = $"Update review {id} successfully.",
                Data = reviewVM
            });

        } catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new ApiResponse
            {
                IsSuccess = false,
                ErrorMessage = ex.Message
            });
        } catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse
            {
                IsSuccess = false,
                ErrorMessage =
                $"**Unexpected Error**: An error occurs when create new review." +
                $"Details {ex.Message}"
            });
        }
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> SoftDeleteReview(Guid id)
    {
        try
        {
            await _reviewService.DeleteReviewAsync(id);
            return Ok(new ApiResponse
            {
                Message = $"Delete review {id} successfully."
            });

        } 
        catch(KeyNotFoundException ex)
        {
            return NotFound(new ApiResponse
            {
                IsSuccess = false,
                ErrorMessage = ex.Message
            });
        } catch (ArgumentException ex)
        {
            return BadRequest(new ApiResponse
            {
                IsSuccess = false,
                ErrorMessage = ex.Message
            });
        } catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new ApiResponse
            {
                IsSuccess = false,
                ErrorMessage = ex.Message
            });
        } catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse
            {
                IsSuccess = false,
                ErrorMessage =
                $"**Unexpected Error**: An error occurs when soft delete the review {id}." +
                $"Details {ex.Message}"
            });
        }
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetReviewById(Guid id)
    {
        try
        {
            var review = await _reviewService.GetByIdAsync(id);
            if (review == null)
            {
                return NotFound(new ApiResponse
                {
                    IsSuccess = false,
                    ErrorMessage = $"Review with id {id} not found."
                });
            }
            return Ok(new ApiResponse
            {
                Data = review
            });
        } catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse
            {
                IsSuccess = false,
                ErrorMessage =
                $"**Unexpected Error**: An error occurs when get review {id}." +
                $"Details: {ex.Message}"
            });
        }
    }
}
