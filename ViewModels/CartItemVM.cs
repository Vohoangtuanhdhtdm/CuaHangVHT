namespace CuaHangVHT.ViewModels
{
    public class CartItemVM
    {
        public int MaHh { get; set; }
        public string Hinh { get; set; }
        public string TenHH { get; set; }
        public decimal DonGia { get; set; }
        public int MaSizes { get; set; }
        public string TenSize { get; set; }
        public int SoLuong { get; set; }
        public decimal ThanhTien => SoLuong * DonGia;
    }
}
