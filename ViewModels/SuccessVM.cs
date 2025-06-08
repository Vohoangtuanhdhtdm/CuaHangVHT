using CuaHangVHT.Data;

namespace CuaHangVHT.ViewModels
{
    public class SuccessVM
    {
        public Order HoaDon { get; set; }
        public List<OrderDetail> ChiTietHoaDon { get; set; }
        public decimal Dicount;
      
        //public List<CartItemVM> GioHangDaMua { get; set; }
        
    }
}
