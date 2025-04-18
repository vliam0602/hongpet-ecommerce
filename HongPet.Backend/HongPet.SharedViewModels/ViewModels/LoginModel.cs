using System.ComponentModel.DataAnnotations;

namespace HongPet.SharedViewModels.ViewModels
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Email không được bỏ trống!")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ!")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mật khẩu không được bỏ trống!")]
        public string Password { get; set; } = string.Empty;
    }
}
