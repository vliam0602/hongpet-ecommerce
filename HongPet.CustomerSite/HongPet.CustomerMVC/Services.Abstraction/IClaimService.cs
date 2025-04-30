namespace HongPet.CustomerMVC.Services.Abstraction;

public interface IClaimService
{
    bool IsAuthorized { get; }
    string AccessToken { get; }
    string RefreshToken { get; }
    Guid? UserId { get; }
    string Email { get; }    
    string Username { get; }    
}
