using MassTransit;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;

namespace HongPet.Domain.Entities.Commons;

public class BaseEntity
{
    [Key]
    public Guid Id { get; set; } = NewId.Next().ToGuid();
    public Guid? CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public Guid? LastModificatedBy { get; set; }
    public DateTime LastModificatedDate { get; set; } = DateTime.UtcNow;

    // For soft delete
    public Guid? DeletedBy { get; set; }
    public DateTime? DeletedDate { get; set; }
}

