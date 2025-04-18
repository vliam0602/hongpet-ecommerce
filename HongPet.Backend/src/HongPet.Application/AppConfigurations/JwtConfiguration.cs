using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HongPet.Application.AppConfigurations;
public class JwtConfiguration
{
    public string SecretKey { get; set; } = default!;
    public string Issuer { get; set; } = default!;
    public string Audience { get; set; } = default!;
    public int ATExpHours { get; set; }
    public int RTExpHours { get; set; }
}
