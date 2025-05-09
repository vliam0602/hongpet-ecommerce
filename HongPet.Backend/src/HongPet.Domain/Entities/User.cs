using HongPet.Domain.Entities.Commons;
using HongPet.Domain.Enums;

namespace HongPet.Domain.Entities;

public class User : BaseEntity
{
    public string Email { get; set; } = default!;
    public string Username { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string Fullname { get; set; } = default!;
    public string? Address { get; set; }
    public string? PhoneNumber { get; set; }
    public string? AvatarUrl { get; set; }
    public RoleEnum Role { get; set; } = RoleEnum.Customer;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
}
