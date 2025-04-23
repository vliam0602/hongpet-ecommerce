namespace HongPet.Application.Services.Commons;
public interface IClaimService
{
    public Guid? GetCurrentUserId { get; }
    public string? GetCurrentEmail { get; }
    public string? GetCurrentRole { get; }
    public bool? IsAdmin { get; }
}
