namespace CuaHangVHT.ViewModels
{
    public class OrderVnPay
    {
        public int UserId { get; set; }
        public string Hoten { get; set; }
        public string DiaChi { get; set; }
        public string DienThoai { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? Status { get; set; }
        public string? CachThanhToan { get; set; }
        public string? CachVanChuyen { get; set; }
        public string? MaTrangThai { get; set; }
        public decimal TotalPrice { get; set; }
        public string? GhiChu { get; set; }
        public int? EmployeeId { get; set; }
    }
}
