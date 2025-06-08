using CuaHangVHT.Data;

namespace CuaHangVHT.Areas.Admin.ViewModel
{
    public class ChiTietDonHangVM
    {
        public Order HoaDon { get; set; }
        public List<OrderDetail> CTHD { get; set; }
        public User KhachHang { get; set; }
    }
}
