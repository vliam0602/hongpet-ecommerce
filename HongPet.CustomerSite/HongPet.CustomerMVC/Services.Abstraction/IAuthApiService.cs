using HongPet.SharedViewModels.Models;

public interface IAuthApiService
{
    Task<TokenModel> LoginAsync(LoginModel loginModel);
    Task<RegisterModel> RegisterAsync(RegisterModel loginModel);
}
