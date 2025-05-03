using System;
using System.Collections.Generic;
using System.Text;

namespace HongPet.SharedViewModels.ViewModels
{
    public class UserVM : BaseVM
    {
        public string Email { get; set; } = default!;
        public string Username { get; set; } = default!;
        public string Fullname { get; set; } = default!;
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public int TotalOrders { get; set; } = 0;
        public decimal TotalSpend { get; set; } = 0;
        public string? AvatarUrl { get; set; }
        public string Role { get; set; } = default!;
    }
}
