using Domain.Entities.Commons;
using HongPet.Domain.Enums;

namespace HongPet.Domain.Entities;
public class User : BaseEntity
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Fullname { get; set; } = string.Empty;
    public string? ShippingAddress { get; set; }
    public string? AvatarUrl { get; set; }
    public RoleEnum Role { get; set; } = RoleEnum.Customer;
}
