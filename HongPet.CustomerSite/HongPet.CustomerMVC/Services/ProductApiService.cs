using HongPet.CustomerMVC.Services.Abstraction;
using HongPet.CustomerMVC.Utilities;
using HongPet.SharedViewModels.Generals;
using HongPet.SharedViewModels.Models;
using HongPet.SharedViewModels.ViewModels;
using Newtonsoft.Json;
using System.Net;

namespace HongPet.CustomerMVC.Services;

public class ProductApiService(
    HttpClient _httpClient) : IProductApiService
{

    public async Task<PagedList<ProductGeneralVM>> GetProductsAsync(QueryListCriteria criteria)
    {
        var url = $"api/products?" +
            $"pageIndex={criteria.PageIndex}" +
            $"&pageSize={criteria.PageSize}" +
            $"&keyword={criteria.Keyword}";

        var (response, apiResponse) = 
            await HttpClientHelper.GetAsync(_httpClient, url);

        if (response.StatusCode == HttpStatusCode.InternalServerError ||
                    !apiResponse.IsSuccess)
        {
            throw new Exception($"Server error: {apiResponse.ErrorMessage}");
        }

        var responseString = apiResponse?.Data?.ToString()!;       

        var products = JsonConvert
            .DeserializeObject<PagedList<ProductGeneralVM>>(responseString);

        if (products == null)
        {
            throw new HttpRequestException("Failed to parse response data.");
        }

        return products;
    }

    public async Task<ProductDetailVM?> GetProductByIdAsync(Guid id)
    {
        var url = $"api/products/{id}";

        var (response, apiResponse) = 
            await HttpClientHelper.GetAsync(_httpClient, url);        

        // Check response status
        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            throw new KeyNotFoundException(apiResponse.ErrorMessage);
        } else if (response.StatusCode == HttpStatusCode.InternalServerError ||
                    !apiResponse.IsSuccess)
        {
            throw new Exception($"Server error: {apiResponse.ErrorMessage}");
        }

        var responseString = apiResponse?.Data?.ToString()!;        

        var product = JsonConvert
            .DeserializeObject<ProductDetailVM>(responseString);
        
        if (product == null)
        {
            throw new HttpRequestException("Failed to parse response data.");
        }

        return product;
    }

    public async Task<PagedList<ReviewVM>> GetProductReviewsAsync(
        Guid productId, int pageIndex, int pageSize)
    {
        var url = $"api/products/{productId}/reviews?" +
            $"pageIndex={pageIndex}" +
            $"&pageSize={pageSize}";

        var (response, apiResponse) = 
            await HttpClientHelper.GetAsync(_httpClient, url);

        if (response.StatusCode == HttpStatusCode.InternalServerError ||
                    !apiResponse.IsSuccess)
        {
            throw new Exception($"Server error: {apiResponse.ErrorMessage}");
        }

        var responseString = apiResponse?.Data?.ToString()!;        

        var reviews = JsonConvert
            .DeserializeObject<PagedList<ReviewVM>>(responseString);

        if (reviews == null)
        {
            throw new HttpRequestException("Failed to parse response data.");
        }

        return reviews;
    }

}
