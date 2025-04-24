using AutoMapper;
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
public class OrdersController(
    ILogger<OrdersController> _logger,
    IOrderService _orderService,
    IMapper _mapper) : ControllerBase
{
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateOrder(
        [FromBody] OrderCreationModel orderModel)
    {
        try
        {
            var order = await _orderService.CreateOrderAsync(orderModel);
            return CreatedAtAction("CreateOrder", new { Id = order.Id }, 
                new ApiResponse
                {
                    Message = "Order created successfully",
                    Data = order
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
            _logger.LogError(ex,
                $"**Unexpected error** Error occurred while creating new order");

            return StatusCode(500, new ApiResponse
            {
                IsSuccess = false,
                ErrorMessage = $"Unexpected error: Error occurred while creating new order. Details: {ex.Message}"
            });
        }
    }


    [HttpGet]
    [Route("/api/users/{userId}/orders/")]
    [Authorize]
    public async Task<IActionResult> GetUserOrders(
        [FromRoute] Guid userId,
        [FromQuery] QueryListCriteria criteria)
    {
        try
        {
            var orders = await _orderService.GetOrdersByCustomerIdAsync(
                    userId, criteria.PageIndex, criteria.PageSize, criteria.Keyword);

            return Ok(new ApiResponse
            {
                IsSuccess = true,
                Data = orders
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
                $"**Unexpected error** Error occurred while getting orders list of userId {User}");
            return StatusCode(500, new ApiResponse
            {
                IsSuccess = false,
                ErrorMessage = $"Unexpected error: Error occurred while getting orders list of userId {User}. Details: {ex.Message}"
            });
        }
    }

    [HttpGet("/admin/api/orders")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllOrders(
        [FromQuery] QueryListCriteria criteria)
    {
        try
        {
            var orders = await _orderService.GetPagedAsync(
                criteria.PageIndex, criteria.PageSize, criteria.Keyword);
            return Ok(new ApiResponse
            {
                IsSuccess = true,
                Data = _mapper.Map<PagedList<OrderVM>>(orders)
            });
        } catch (Exception ex)
        {
            _logger.LogError(ex, $"**Unexpected error:** " +
                $"Error occurred while getting all orders of admin features.");
            return StatusCode(500, new ApiResponse
            {
                IsSuccess = false,
                ErrorMessage = $"Unexpected error: Error occurred while getting all orders. Details: {ex.Message}"
            });
        }
    }


    [HttpGet("/admin/api/orders/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetOrderDetail(Guid id)
    {
        try
        {
            var order = await _orderService.GetOrderWithDetailsAsync(id);
            return Ok(new ApiResponse
            {
                IsSuccess = true,
                Data = order
            });
        } 
        catch(KeyNotFoundException ex)
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
            _logger.LogError(ex, $"**Unexpected error:** " +
                $"Error occurred while getting all orders of admin features.");
            return StatusCode(500, new ApiResponse
            {
                IsSuccess = false,
                ErrorMessage = $"Unexpected error: Error occurred while getting all orders. Details: {ex.Message}"
            });
        }
    }
}
