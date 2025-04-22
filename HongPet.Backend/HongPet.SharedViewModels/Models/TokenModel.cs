using System;
using System.Collections.Generic;
using System.Text;

namespace HongPet.SharedViewModels.Models
{
    public class TokenModel
    {
        public string AccessToken { get; set; } = default!;
        public string RefreshToken { get; set; } = default!;
    }
}
