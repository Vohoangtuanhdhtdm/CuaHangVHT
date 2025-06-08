using System.ComponentModel.DataAnnotations;

namespace CuaHangVHT.ViewModels
{
    // Nếu giống Email đã đăng ký trước đó thì sao? => lấy lại tài khoản đã đăng ký | email đã đc đăng ký Quên Mật khẩu?
    public class DangKyViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập họ và tên.")]
        [Display(Name = "Bạn tên gì..")]
        [StringLength(60, ErrorMessage = "Họ và tên không được vượt quá 60 ký tự.")]
        public string full_name { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập địa chỉ.")]
        [Display(Name = "Địa chỉ..")]
        public string address { get; set; }

        [Required]
        [Display(Name = "Số điện thoại..")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ.")]
        [RegularExpression(@"^(\d{10,11})$", ErrorMessage = "Số điện thoại phải có từ 10 đến 11 chữ số.")]
        public string phone_number { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên đăng nhập.")]
        [Display(Name = "Tên đăng nhập")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Tên đăng nhập phải có từ 3 đến 50 ký tự.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập email.")]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu.")]
        [Display(Name = "Mật khẩu")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Mật khẩu phải có từ 6 ký tự trở lên.")]
        public string Password { get; set; }

        [Display(Name = "Ảnh đại diện")]
        public IFormFile? ImgAvater { get; set; }
    }
}

//[Required(ErrorMessage = "Vui lòng nhập mật khẩu.")]
//[Display(Name = "Mật khẩu")]
//[DataType(DataType.Password)]
//[StringLength(100, MinimumLength = 6, ErrorMessage = "Mật khẩu phải có từ 6 ký tự trở lên.")]
//[RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,}$",
//    ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự, bao gồm ít nhất 1 chữ cái in hoa, 1 chữ cái in thường, 1 chữ số và 1 ký tự đặc biệt.")]
//public string Password { get; set; }
