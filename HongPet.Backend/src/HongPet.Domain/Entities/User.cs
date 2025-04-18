using Domain.Entities.Commons;
using HongPet.Domain.Enums;

namespace HongPet.Domain.Entities;

public class User : BaseEntity
{    
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Fullname { get; set; } = string.Empty;
    public string? Address { get; set; }
    public string? PhoneNumber { get; set; }
    public string? AvatarUrl { get; set; }
    public RoleEnum Role { get; set; } = RoleEnum.Customer;

    public virtual IEnumerable<Order> Orders { get; set; } = new List<Order>();
    public virtual IEnumerable<Review> Reviews { get; set; } = new List<Review>();
    public virtual UserToken? UserToken { get; set; }
}
