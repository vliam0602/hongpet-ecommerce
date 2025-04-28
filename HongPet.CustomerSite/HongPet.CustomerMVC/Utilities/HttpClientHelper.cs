using HongPet.SharedViewModels.Models;
using Newtonsoft.Json;

namespace HongPet.CustomerMVC.Utilities;

public static class HttpClientHelper
{
    public static async Task<(HttpResponseMessage, ApiResponse)> GetAsync(
        HttpClient httpClient, string url)
    {
        var response = await httpClient.GetAsync(url);
        var apiResponse = await ParseApiResponseAsync(response);
        return (response, apiResponse);
    }

    public static async Task<(HttpResponseMessage, ApiResponse)> PostAsync<T>(
        HttpClient httpClient, string url, T content)
    {
        var response = await httpClient.PostAsJsonAsync(url, content);
        var apiResponse = await ParseApiResponseAsync(response);
        return (response, apiResponse);
    }

    private static async Task<ApiResponse> ParseApiResponseAsync(
        HttpResponseMessage response)
    {
        var responseContent = await response.Content.ReadAsStringAsync();

        var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(responseContent);

        if (apiResponse == null)
        {
            throw new HttpRequestException(apiResponse?.ErrorMessage ??
                "Invalid response from server.");
        }

        return apiResponse;
    }
}
