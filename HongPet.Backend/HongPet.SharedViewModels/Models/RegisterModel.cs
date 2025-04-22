using System.ComponentModel.DataAnnotations;

namespace HongPet.SharedViewModels.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Họ tên không được bỏ trống!")]
        public string Fullname { get; set; } = default!;

        [Required(ErrorMessage = "Email không được bỏ trống!")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ!")]
        public string Email { get; set; } = default!;

        [Required(ErrorMessage = "Mật khẩu không được bỏ trống!")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Xác nhận mật khẩu không được bỏ trống!")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
