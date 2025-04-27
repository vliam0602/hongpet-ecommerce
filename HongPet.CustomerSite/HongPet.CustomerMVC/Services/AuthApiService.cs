using HongPet.SharedViewModels.Models;
using Newtonsoft.Json;
using System.Net;

namespace HongPet.CustomerMVC.Services;

public class AuthApiService(
    HttpClient _httpClient) : IAuthApiService
{
    public async Task<TokenModel> LoginAsync(LoginModel loginModel)
    {
        var response = await _httpClient
            .PostAsJsonAsync("api/auth/login", loginModel);        

        // Convert response content to ApiResponse
        var responseContent = await response.Content.ReadAsStringAsync();
        var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(responseContent);

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
        // Gửi yêu cầu POST đến API /api/auth/register
        var response = await _httpClient.PostAsJsonAsync("api/auth/register", registerModel);

        // Đọc nội dung phản hồi từ API
        var responseContent = await response.Content.ReadAsStringAsync();
        var apiResponse = JsonConvert
            .DeserializeObject<ApiResponse>(responseContent);

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
