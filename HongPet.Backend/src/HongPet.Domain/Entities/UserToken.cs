using Domain.Entities.Commons;
using System.Security.Principal;

namespace HongPet.Domain.Entities;
public class UserToken : BaseEntity
{
    public Guid UserId { get; set; }
    public Guid ATid { get; set; } = default!;
    public Guid RTid { get; set; } = default!;
    public string RefreshToken { get; set; } = default!;
    //public bool IsUsed { get; set; } = false;
    //public bool IsRevoked { get; set; } = false;
    public DateTime IssuedDate { get; set; }
    public DateTime ExpiredDate { get; set; }

    public virtual User User { get; set; } = default!;
}
