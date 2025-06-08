using CuaHangVHT.Data;
using System.Drawing;

namespace CuaHangVHT.ViewModels
{
    public class HangHoaVM
    {
        public int MaHh { get; set; }
        public string TenHH { get; set; }  
        public string Hinh { get; set;}
        public decimal DonGia { get; set; }
        public string MoTaNgan { get; set;}
        public decimal? PhanTramGiamGia { get; set; }
     

    }

    public class ChiTietHangHoaVM
    {
        public int MaHh { get; set; }
        public string TenHH { get; set; }
        public string Hinh { get; set; }
        public decimal DonGia { get; set; }
        public string MoTaNgan { get; set; }
        public string TenLoai { get; set; }
        public int soLuong { get; set; }
      public ICollection<ProductSizeVM> Sizes { get; set; }
    }
}

