using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HongPet.Application.Services.Commons;
public interface IClaimService
{
    public Guid? GetCurrentUserId { get; }
    public string GetCurrentEmail{ get; }
    public string GetCurrentRole { get; }
    public bool IsAdmin();
}
