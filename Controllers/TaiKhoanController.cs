using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using CuaHangVHT.Data;
using CuaHangVHT.Helper;
using CuaHangVHT.Services;
using CuaHangVHT.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CuaHangVHT.Controllers
{
    public class TaiKhoanController : Controller
    {
        private readonly TuanStoreContext db;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly INotyfService _notyf;
        public TaiKhoanController(TuanStoreContext context, IMapper mapper, IEmailService emailService, INotyfService notyf)
        {
            db = context;
            _mapper = mapper;
            _emailService = emailService;
            _notyf = notyf;
        }

        #region Đăng ký 

        [HttpGet]
        public IActionResult Dangky()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index","Home" );
            }
            return View();
        }

        [HttpPost]
        public IActionResult Dangky(DangKyViewModel model, IFormFile? ImgAvater)
        {
            if (!ModelState.IsValid) return View();
            // Check if the email already exists
            bool emailExists = db.Users.Any(u => u.Email == model.Email && u.Username == model.Username);
            if (emailExists)
            {
                _notyf.Error("Email này đã được đăng ký");
                //ModelState.AddModelError("Email", "Email này đã được đăng ký."); // Validation Email
                return View(model);
            }
            try
            {
                var khachHang = _mapper.Map<User>(model);
                khachHang.FullName = model.full_name;
                khachHang.PhoneNumber = model.phone_number;
                khachHang.RandomKey = MyUtil.GenerateRamdomKey();
                khachHang.Password = model.Password.ToMd5Hash(khachHang.RandomKey);
                khachHang.ImgAvater = model.ImgAvater.FileName.ToString();
                khachHang.Role = "customer";

                if (ImgAvater != null)
                {
                    //khachHang.ImgAvater = MyUtil.UploadHinh(ImgAvater, "customer");
                    MyUtil.UploadHinh(ImgAvater, "customer");
                }

                db.Add(khachHang);
                db.SaveChanges();
                _notyf.Success("Đăng kí thành công!");
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Có lỗi xảy ra trong quá trình đăng ký.");
                Console.WriteLine($"Error: {ex.Message}");
            }
            return View();
        }

        #endregion

        #region Đăng nhập

        [HttpGet]
        public IActionResult DangNhap(string? ReturnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DangNhap(DangNhapViewModel model, string? ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            if (!ModelState.IsValid) return View();

            var NgDung = db.Users.SingleOrDefault(kh => kh.Email == model.Email);

            if (NgDung == null)
            {
                ModelState.AddModelError("Lỗi", "Không tồn tại tài khoản");
                _notyf.Error("Không tồn tại tài khoản");
            }
            else
            {

                // Fix thành công: "do nó tự dộng xuất hiện nhiều khoảng trắng => dùng hàm.Trim()"
                string hashedPassword = model.Password.ToMd5Hash(NgDung.RandomKey.Trim());

                if (NgDung.Password != hashedPassword)
                {
                    ModelState.AddModelError("loi", "Sai thông tin đăng nhập");
                    _notyf.Error("Sai thông tin đăng nhập.");
                }
                else
                {
                    // Lấy vai trò người dùng
                    var roleEmp = db.Employees
                        .Where(em => em.UserId == NgDung.UserId)
                        .Select(em => new
                        {
                            em.PositionId,
                            PositionName = db.Positions.Where(p => p.PositionId == em.PositionId).Select(p => p.PositionName).FirstOrDefault()
                        })
                        .FirstOrDefault();

                    var POS = roleEmp?.PositionName ?? "Customer";

                    // Tạo danh sách các claim cho người dùng
                    var claims = new List<Claim> {
                                new Claim(ClaimTypes.Email, NgDung.Email),
                                new Claim(ClaimTypes.Name, NgDung.FullName),
                                new Claim(MySetting.CLAIM_CUSTOMERID, NgDung.UserId.ToString()),
                                 new Claim(ClaimTypes.Role, POS) // Role động dựa trên thuộc tính Positon
                    };
                    // Tạo ClaimsIdentity và ClaimsPrincipal
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                    /* 
                      - ClaimsPrincipal: là tập hợp của các ClaimsIdentity, trong đó bao gồm 
                     danh tính của người dùng.
                    
                      - ClaimsIdentity: đại diện cho danh tính người dùng dựa trên claims vừa tạo
                     */

                    // Đăng nhập người dùng vào hệ thống
                    await HttpContext.SignInAsync(claimsPrincipal);
                    _notyf.Success("Đăng nhập thành công!");
                    // Điều hướng đến ReturnUrl hoặc trang chủ
                    return Url.IsLocalUrl(ReturnUrl) ? Redirect(ReturnUrl) : Redirect("/");
                }
            }

            return View();
        }


        #endregion

        #region Quên mật khẩu 

        public IActionResult QuenMatKhau()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> QuenMatKhau(string email)
        {
            var user = await db.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                 _notyf.Error("Không tìm thấy email này trong hệ thống!");
                //ModelState.AddModelError("", "Không tìm thấy email này trong hệ thống.");
                return View();
            }

            var token = Guid.NewGuid().ToString(); // tạo token
            user.ResetPasswordToken = token;
            user.TokenExpiration = DateTime.Now.AddHours(1);
            await db.SaveChangesAsync();

            string resetLink = Url.Action("DatLaiMatKhau", "TaiKhoan", new { token = token, email = email }, Request.Scheme);
            string emailBody = $"Click vào liên kết sau để đặt lại mật khẩu của bạn: <a href='{resetLink}'>Đặt lại mật khẩu</a>";

            await _emailService.SendEmailAsync(email, "Quên mật khẩu", emailBody);

            _notyf.Success("Email đặt lại mật khẩu đã được gửi. Vui lòng check mail của bạn!");
            return RedirectToAction("DangNhap");
        }
        #endregion

        #region Đặt lại mật khẩu

        // GET: Hiển thị form đặt lại mật khẩu
        [HttpGet]
        public IActionResult DatLaiMatKhau(string token, string email)
        {
            // Kiểm tra xem token và email có được truyền vào hay không
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(email))
            {
                _notyf.Error("Đã xảy ra lỗi trong quá trình đặt lại mật khẩu !");
                return RedirectToAction("Index", "Home"); // Quay lại trang chủ nếu không có token hoặc email
            }
            var model = new DatLaiMatKhauViewModel { Token = token, Email = email };
            return View(model);
        }

        // POST: Xử lý việc đặt lại mật khẩu
        [HttpPost]
        public async Task<IActionResult> DatLaiMatKhau(DatLaiMatKhauViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await db.Users.FirstOrDefaultAsync(u => u.Email == model.Email && u.ResetPasswordToken == model.Token && u.TokenExpiration > DateTime.Now);
            if (user == null)
            {
                _notyf.Error("Liên kết đặt lại mật khẩu không hợp lệ hoặc đã hết hạn.");
                // ModelState.AddModelError("", "Liên kết đặt lại mật khẩu không hợp lệ hoặc đã hết hạn.");
                return View();
            }

            // Đặt lại mật khẩu mới
            user.Password = model.NewPassword.ToMd5Hash(user.RandomKey.Trim()); //lỗi khoảng trắng
            user.ResetPasswordToken = null; // Xóa token sau khi đã sử dụng
            user.TokenExpiration = null;
            db.Update(user);
            await db.SaveChangesAsync();
            _notyf.Success("Mật khẩu của bạn đã được đặt lại thành công!");
          
            return RedirectToAction("DangNhap");
        }

        #endregion

        [Authorize]
        public IActionResult Profile(int? id)
        {
            // Sử dụng phương thức mở rộng để lấy các giá trị claim và truyền vào ViewData
            ViewData["Email"] = User.GetClaimValue(ClaimTypes.Email);
			ViewData["Name"] = User.GetClaimValue(ClaimTypes.Name);
            var CustomerId = User.GetClaimValue(MySetting.CLAIM_CUSTOMERID);
            var us = db.Users.SingleOrDefault(kh => kh.UserId == int.Parse(CustomerId));
            ViewData["CustomerAdress"] = us.Address;
            ViewData["ImgAvartar"] = us.ImgAvater;
            ViewData["DienThoai"] = us.PhoneNumber;
            ViewData["Role"] = User.GetClaimValue(ClaimTypes.Role);

			return View();
        }

        [Authorize]
        public async Task<IActionResult> DangXuat()
        {
            await HttpContext.SignOutAsync();
            _notyf.Success("Đăng xuất thành công.");
            return Redirect("/");
        }
    }
}
