using System;
using System.Collections.Generic;
using System.Text;

namespace HongPet.SharedViewModels.Models
{
    public class UserUpdateModel
    {
        public string Email { get; set; } = default!;
        public string Username { get; set; } = default!;
        public string Fullname { get; set; } = default!;
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
