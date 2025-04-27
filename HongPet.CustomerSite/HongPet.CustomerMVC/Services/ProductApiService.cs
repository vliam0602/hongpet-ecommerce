using HongPet.CustomerMVC.Services.Abstraction;
using HongPet.CustomerMVC.Utilities;
using HongPet.SharedViewModels.Generals;
using HongPet.SharedViewModels.Models;
using HongPet.SharedViewModels.ViewModels;
using Newtonsoft.Json;
using System.Net;

namespace HongPet.CustomerMVC.Services;

public class ProductApiService : IProductApiService
{
    private readonly HttpClient _httpClient;

    public ProductApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<PagedList<ProductGeneralVM>> GetProductsAsync(QueryListCriteria criteria)
    {
        var url = $"api/products?" +
            $"pageIndex={criteria.PageIndex}" +
            $"&pageSize={criteria.PageSize}" +
            $"&keyword={criteria.Keyword}";

        var (response, apiResponse) = 
            await HttpClientHelper.GetAsync(_httpClient, url);

        if (apiResponse == null)
        {
            throw new HttpRequestException(apiResponse?.ErrorMessage ??
                "Invalid response from server.");
        }

        if (response.StatusCode == HttpStatusCode.InternalServerError)
        {
            throw new Exception($"Server error: {apiResponse.ErrorMessage}");
        }

        var responseString = apiResponse?.Data?.ToString()!;       

        var products = JsonConvert
            .DeserializeObject<PagedList<ProductGeneralVM>>(responseString);

        return products ?? new PagedList<ProductGeneralVM>();
    }

    public async Task<ProductDetailVM?> GetProductByIdAsync(Guid id)
    {
        var url = $"api/products/{id}";

        var (response, apiResponse) = 
            await HttpClientHelper.GetAsync(_httpClient, url);        

        if (apiResponse == null)
        {
            throw new HttpRequestException(apiResponse?.ErrorMessage ??
                "Invalid response from server.");
        }

        // Check response status
        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            throw new KeyNotFoundException(apiResponse.ErrorMessage);
        } else if (response.StatusCode == HttpStatusCode.InternalServerError)
        {
            throw new Exception($"Server error: {apiResponse.ErrorMessage}");
        }

        var responseString = apiResponse?.Data?.ToString()!;

        if (string.IsNullOrEmpty(responseString))
        {
            throw new HttpRequestException("Failed to parse response data.");
        }

        var products = JsonConvert
            .DeserializeObject<ProductDetailVM>(responseString);

        return products;
    }

    public async Task<PagedList<ReviewVM>> GetProductReviewsAsync(Guid productId, QueryListCriteria criteria)
    {
        //var response = await _httpClient.GetFromJsonAsync<ApiResponse>(
        //    $"api/products/{productId}/reviews?" +
        //    $"pageIndex={criteria.PageIndex}" +
        //    $"&pageSize={criteria.PageSize}");

        //if (response == null)
        //{
        //    throw new HttpRequestException(
        //        $"Error when request reviews of product with id {productId}. " +
        //        $"The api response is null.");
        //}

        //return (PagedList<ReviewVM>)response.Data!;
        throw new NotImplementedException();
    }
}
