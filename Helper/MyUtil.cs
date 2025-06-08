using CuaHangVHT.Data;
using System.Security.Claims;
using System.Text;

namespace CuaHangVHT.Helper
{
    public static class MyUtil
    {
        public static string UploadHinh(IFormFile Hinh, string folder)
     {
//            - IFormFile Hinh: Đây là đối tượng đại diện cho file mà người dùng
//            tải lên(thường là từ một form trong một ứng dụng web).
//            - string folder: Tên thư mục nơi file sẽ được lưu trữ.
            try
            {
                var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "assets", folder, Hinh.FileName);
                using (var myfile = new FileStream(fullPath, FileMode.CreateNew))
                {
                    Hinh.CopyTo(myfile);
                }
                return Hinh.FileName;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
        public static string GenerateRamdomKey(int length = 5)
        {
            var pattern = @"qazwsxedcrfvtgbyhnujmiklopQAZWSXEDCRFVTGBYHNUJMIKLOP!";
            var sb = new StringBuilder();
            var rd = new Random();
            for (int i = 0; i < length; i++)
            {
                sb.Append(pattern[rd.Next(0, pattern.Length)]);
            }
            return sb.ToString();
        }
        
        public static string GetClaimValue(this ClaimsPrincipal user, string claimType)
        {
			return user?.Claims.FirstOrDefault(c => c.Type == claimType)?.Value;
		}
	}
}
