using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HongPet.SharedViewModels.ViewModels
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Email không được bỏ trống!")]
        public string Email { get; set; } = string.Empty;
        [Required(ErrorMessage = "Mật khẩu không được bỏ trống!")]
        public string Password { get; set; } = string.Empty;
    }
}
