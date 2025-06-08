using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace CuaHangVHT.ViewModels
{
    public class DangNhapViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập email.")]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Tên đăng nhập ít nhất phải từ 6 ký tự.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu.")]
        [Display(Name = "Mật khẩu")]
        [DataType(DataType.Password)]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Mật khẩu ít nhất phải từ 6 ký tự")]
       // [StrongPassword(ErrorMessage = "Mật khẩu quá yếu, cần ít nhất 1 chữ hoa, 1 chữ thường, 1 số và 1 ký tự đặc biệt.")]
        public string Password { get; set; }

    }
    //// Custom Attribute để kiểm tra độ mạnh mật khẩu
    //public class StrongPasswordAttribute : ValidationAttribute
    //{
    //    public override bool IsValid(object value)
    //    {
    //        if (value == null) return false;

    //        var password = value.ToString();

    //        // Tối thiểu: 1 chữ hoa, 1 chữ thường, 1 số, 1 ký tự đặc biệt
    //        return Regex.IsMatch(password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).+$");
    //    }
    //}
}
