using System.ComponentModel.DataAnnotations;

namespace HongPet.SharedViewModels.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Email không được bỏ trống!")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ!")]
        public string Email { get; set; } = default!;

        [Required(ErrorMessage = "Mật khẩu không được bỏ trống!")]
        public string Password { get; set; } = string.Empty;
    }
}