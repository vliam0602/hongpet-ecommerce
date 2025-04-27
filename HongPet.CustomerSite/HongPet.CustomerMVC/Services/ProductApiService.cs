using HongPet.CustomerMVC.Services.Abstraction;
using HongPet.SharedViewModels.Generals;
using HongPet.SharedViewModels.Models;
using HongPet.SharedViewModels.ViewModels;
using Newtonsoft.Json;

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
        var response = await _httpClient
            .GetFromJsonAsync<ApiResponse>(
            $"api/products?" +
            $"pageIndex={criteria.PageIndex}" +
            $"&pageSize={criteria.PageSize}" +
            $"&keyword={criteria.Keyword}");

        var responseString = response?.Data?.ToString()!;

        if (response == null|| string.IsNullOrEmpty(responseString))
        {
            throw new HttpRequestException(
                $"Error when requesting products. " +
                $"The API response is empty.");
        }

        var products = JsonConvert
            .DeserializeObject<PagedList<ProductGeneralVM>>(responseString);

        return products ?? new PagedList<ProductGeneralVM>();
    }

    public async Task<ProductGeneralVM> GetProductAsync(Guid id)
    {
        //var response = await _httpClient
        //    .GetFromJsonAsync<ApiResponse>(
        //    $"api/products/{id}");

        //if (response == null)
        //{
        //    throw new HttpRequestException(
        //        $"Error when request product with id {id}. " +
        //        $"The api response is null.");
        //}

        //return (ProductGeneralVM)response.Data!;
        throw new NotImplementedException();
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
