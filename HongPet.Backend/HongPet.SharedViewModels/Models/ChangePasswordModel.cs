using System;
using System.Collections.Generic;
using System.Text;

namespace HongPet.SharedViewModels.Models
{
    public class ChangePasswordModel
    {
        public string OldPassword { get; set; } = default!;
        public string NewPassword { get; set; } = default!;
    }
}
