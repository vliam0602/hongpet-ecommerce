using HongPet.CustomerMVC.Utilities;
using HongPet.SharedViewModels.Models;
using Newtonsoft.Json;
using System;
using System.Net;

namespace HongPet.CustomerMVC.Services;

public class AuthApiService(
    HttpClient _httpClient) : IAuthApiService
{
    public async Task<TokenModel> LoginAsync(LoginModel loginModel)
    {
        var url = $"api/auth/login";

        var (response, apiResponse) = await HttpClientHelper
            .PostAsync(_httpClient, url, loginModel);                

        if (apiResponse == null)
        {
            throw new HttpRequestException(apiResponse?.ErrorMessage ??
                "Invalid response from server.");
        }

        // Check response status
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            throw new UnauthorizedAccessException($"Login failed: {apiResponse.ErrorMessage}");
        } else if (response.StatusCode == HttpStatusCode.InternalServerError)
        {
            throw new Exception($"Server error: {apiResponse.ErrorMessage}");
        }

        // Convert Data to TokenModel
        var tokenModel = JsonConvert.DeserializeObject<TokenModel>(apiResponse.Data!.ToString()!);
        if (tokenModel == null)
        {
            throw new HttpRequestException("Failed to parse token data.");
        }

        return tokenModel;
    }

    public async Task<RegisterModel> RegisterAsync(RegisterModel registerModel)
    {        
        var url = "api/auth/register";
        var (response, apiResponse) = await HttpClientHelper
            .PostAsync(_httpClient, url, registerModel);

        if (apiResponse == null)
        {
            throw new HttpRequestException(apiResponse?.ErrorMessage ??
                "Invalid response from server.");
        }

        if (apiResponse == null)
        {
            throw new HttpRequestException(apiResponse?.ErrorMessage ?? 
                $"Invalid response from server. {response}");
        }

        // Kiểm tra trạng thái phản hồi
        if (response.StatusCode == HttpStatusCode.BadRequest)
        {
            throw new ArgumentException(apiResponse.ErrorMessage);
        } else if (response.StatusCode == HttpStatusCode.InternalServerError)
        {
            throw new Exception($"Server error: {apiResponse.ErrorMessage}");
        }

        // Chuyển đổi dữ liệu phản hồi thành RegisterModel
        var registeredUser = JsonConvert
            .DeserializeObject<RegisterModel>(apiResponse.Data!.ToString()!);

        if (registeredUser == null)
        {
            throw new HttpRequestException("Failed to parse registered user data.");
        }

        return registeredUser;
    }

}
