using HongPet.Application.Services.Abstractions;
using HongPet.SharedViewModels.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HongPet.WebApi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UsersController(
            ILogger<UsersController> _logger,
            IUserService _userService) : ControllerBase
{
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetUsers([FromQuery] QueryListCriteria criteria)
    {
        try
        {
            var users = await _userService.GetUsersListAsync(criteria.PageIndex, criteria.PageSize, criteria.Keyword);
            return Ok(new ApiResponse
            {
                IsSuccess = true,
                Data = users
            });
        } catch (Exception ex)
        {
            _logger.LogError(ex, 
                "**Unexpected error** Error occurred while getting users list.");
            return StatusCode(500, new ApiResponse
            {
                IsSuccess = false,
                ErrorMessage = $"Unexpected error: Error occurred while getting users list. Details: {ex.Message}"
            });
        }
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetUserDetail(Guid id)
    {
        try
        {
            var user = await _userService.GetUserDetailAsync(id);
            return Ok(new ApiResponse
            {
                IsSuccess = true,
                Data = user
            });
        } catch (KeyNotFoundException ex)
        {
            return NotFound(new ApiResponse
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
            _logger.LogError(ex, 
                $"**Unexpected error** Error occurred while getting user with id {id}.");
            return StatusCode(500, new ApiResponse
            {
                IsSuccess = false,
                ErrorMessage = $"Unexpected error: Error occurred while getting user with id {id}. Details: {ex.Message}"
            });
        }
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> UpdateUser(Guid id, 
        [FromBody] UserUpdateModel userModel)
    {
        try
        {
            var updatedUser = await _userService.UpdateUserInfoAsync(id, userModel);
            return Ok(new ApiResponse
            {
                Message = $"User {id} updated successfully.",
                Data = updatedUser
            });
        } catch (KeyNotFoundException ex)
        {
            return NotFound(new ApiResponse
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
            _logger.LogError(ex, 
                $"**Unexpected error** Error occurred while updating user with id {id}.");
            return StatusCode(500, new ApiResponse
            {
                IsSuccess = false,
                ErrorMessage = $"Unexpected error: Error occurred while updating user with id {id}. Details: {ex.Message}"
            });
        }
    }

    [HttpPut("{id}/avatar")]
    [Authorize]
    public async Task<IActionResult> UpdateAvatar(Guid id, 
        [FromBody] UserUpdateAvatarModel model)
    {
        try
        {
            var result = await _userService.UpdateAvatarAsync(id, model.AvatarUrl);
            return Ok(new ApiResponse
            {
                Message = $"Avatar for user {id} updated successfully.",
                Data = result
            });
        } catch (KeyNotFoundException ex)
        {
            return NotFound(new ApiResponse
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
            _logger.LogError(ex, 
                $"**Unexpected error** Error occurred while updating avatar for user with id {id}.");
            return StatusCode(500, new ApiResponse
            {
                IsSuccess = false,
                ErrorMessage = 
                $"Unexpected error: Error occurred while updating avatar for user with id {id}. Details: {ex.Message}"
            });
        }
    }

    [HttpPut("{id}/password")]
    [Authorize]
    public async Task<IActionResult> ChangePassword(Guid id, 
        [FromBody] ChangePasswordModel model)
    {
        try
        {
            var result = await _userService
                .ChangePasswordAsync(id, model.OldPassword, model.NewPassword);
            return Ok(new ApiResponse
            {
                Message = $"Password for user {id} changed successfully.",
                Data = result
            });
        } catch (KeyNotFoundException ex)
        {
            return NotFound(new ApiResponse
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
        } catch (ArgumentException ex)
        {
            return BadRequest(new ApiResponse
            {
                IsSuccess = false,
                ErrorMessage = ex.Message
            });
        } catch (Exception ex)
        {
            _logger.LogError(ex, $"**Unexpected error** Error occurred while changing password for user with id {id}.");
            return StatusCode(500, new ApiResponse
            {
                IsSuccess = false,
                ErrorMessage = $"Unexpected error: Error occurred while changing password for user with id {id}. Details: {ex.Message}"
            });
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> InactiveUser(Guid id)
    {
        try
        {
            await _userService.InactiveUserAsync(id);
            return Ok(new ApiResponse
            {
                Message = $"User {id} deactivated successfully."
            });
        } catch (KeyNotFoundException ex)
        {
            return NotFound(new ApiResponse
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
            _logger.LogError(ex, $"**Unexpected error** Error occurred while deactivating user with id {id}.");
            return StatusCode(500, new ApiResponse
            {
                IsSuccess = false,
                ErrorMessage = $"Unexpected error: Error occurred while deactivating user with id {id}. Details: {ex.Message}"
            });
        }
    }
}
