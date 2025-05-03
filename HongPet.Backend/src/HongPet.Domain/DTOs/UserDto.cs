using MassTransit;

namespace HongPet.Domain.DTOs;
public class UserDto
{
    public Guid Id { get; set; } = NewId.Next().ToGuid();
    public string Email { get; set; } = default!;
    public string Username { get; set; } = default!;
    public string Fullname { get; set; } = default!;
    public string? Address { get; set; }
    public string? PhoneNumber { get; set; }
    public string? AvatarUrl { get; set; }
    public int TotalOrders { get; set; } = 0;
    public decimal TotalSpend { get; set; } = 0;
    public string Role { get; set; } = default!;    
    public Guid? CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public Guid? LastModificatedBy { get; set; }
    public DateTime LastModificatedDate { get; set; }

    // For soft delete
    public Guid? DeletedBy { get; set; }
    public DateTime? DeletedDate { get; set; }
}
