using HongPet.SharedViewModels.Models;
using Newtonsoft.Json;
using NuGet.Common;
using System.Net;
using System.Net.Http.Headers;

namespace HongPet.CustomerMVC.Utilities;

public static class HttpClientHelper
{
    public static async Task<(HttpResponseMessage, ApiResponse)> GetAsync(
        HttpClient httpClient, string url, string token = "")
    {
        if (!string.IsNullOrEmpty(token))
        {
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }
        var response = await httpClient.GetAsync(url);
        var apiResponse = await ParseApiResponseAsync(response);
        return (response, apiResponse);
    }

    public static async Task<(HttpResponseMessage, ApiResponse)> PostAsync<T>(
        HttpClient httpClient, string url, T content, string token = "")
    {
        if (!string.IsNullOrEmpty(token))
        {
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }
        var response = await httpClient.PostAsJsonAsync(url, content);
        var apiResponse = await ParseApiResponseAsync(response);
        return (response, apiResponse);
    }

    private static async Task<ApiResponse> ParseApiResponseAsync(
        HttpResponseMessage response)
    {
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            throw new UnauthorizedAccessException();
        }

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
