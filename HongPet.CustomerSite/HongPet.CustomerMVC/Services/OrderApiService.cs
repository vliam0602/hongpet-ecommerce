using HongPet.CustomerMVC.Services.Abstraction;
using HongPet.CustomerMVC.Utilities;
using HongPet.SharedViewModels.Models;
using System.Net.Http;
using System.Net;
using HongPet.SharedViewModels.ViewModels;
using Newtonsoft.Json;
using HongPet.SharedViewModels.Generals;

namespace HongPet.CustomerMVC.Services;

public class OrderApiService(
    HttpClient _httpClient,
    IClaimService _claimService) : IOrderApiService
{
    public async Task<Guid> CreateOrderAsync(OrderCreationModel orderModel)
    {
        var url = "api/orders";

        var (response, apiResponse) = await HttpClientHelper
            .PostAsync(_httpClient, url, orderModel, _claimService.AccessToken);

        // Kiểm tra trạng thái phản hồi
        if (response.StatusCode == HttpStatusCode.BadRequest)
        {
            throw new ArgumentException(apiResponse.ErrorMessage);
        }
        else if (response.StatusCode == HttpStatusCode.InternalServerError)
        {
            throw new Exception($"Server error: {apiResponse.ErrorMessage}");
        }

        if (!apiResponse.IsSuccess)
        {
            throw new HttpRequestException($"Failed to create order: " +
                $"{apiResponse.ErrorMessage}");
        }
        var responseString = apiResponse?.Data?.ToString()!;
        var orderId = Guid.Parse(responseString);
        if (orderId == default)
        {
            throw new HttpRequestException("Failed to parse response data.");
        }
        return orderId;
    }

    public async Task<PagedList<OrderVM>> GetUserOrdersAsync(Guid userId, 
        QueryListCriteria criteria)
    {
        var url = $"api/users/{userId}/orders?" +
                  $"pageIndex={criteria.PageIndex}" +
                  $"&pageSize={criteria.PageSize}" +
                  $"&keyword={criteria.Keyword}";

        var (response, apiResponse) = await HttpClientHelper
            .GetAsync(_httpClient, url, _claimService.AccessToken);

        if (response.StatusCode == HttpStatusCode.InternalServerError || 
            !apiResponse.IsSuccess)
        {
            throw new Exception($"Server error: {apiResponse.ErrorMessage}");
        }

        var responseString = apiResponse?.Data?.ToString()!;
        var orders = JsonConvert
            .DeserializeObject<PagedList<OrderVM>>(responseString);

        if (orders == null)
        {
            throw new HttpRequestException("Failed to parse response data.");
        }

        return orders;
    }

    public async Task<OrderVM> GetOrderByIdAsync(Guid orderId)
    {
        var url = $"api/orders/{orderId}";

        var (response, apiResponse) = await HttpClientHelper
            .GetAsync(_httpClient, url, _claimService.AccessToken);

        // Kiểm tra trạng thái phản hồi
        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            throw new KeyNotFoundException(apiResponse.ErrorMessage);
        } 
        else if (response.StatusCode == HttpStatusCode.InternalServerError || 
            !apiResponse.IsSuccess)
        {
            throw new Exception($"Server error: {apiResponse.ErrorMessage}");
        }

        var responseString = apiResponse?.Data?.ToString()!;
        var order = JsonConvert.DeserializeObject<OrderVM>(responseString);

        if (order == null)
        {
            throw new HttpRequestException("Failed to parse response data.");
        }

        return order;
    }
}
