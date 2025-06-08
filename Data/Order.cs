using System;
using System.Collections.Generic;

namespace CuaHangVHT.Data;

public partial class Order
{
    public int OrderId { get; set; }

    public int UserId { get; set; }

    public decimal TotalPrice { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? Hoten { get; set; }

    public string? DiaChi { get; set; }

    public string? CachThanhToan { get; set; }

    public string? CachVanChuyen { get; set; }

    public string? DienThoai { get; set; }

    public string? MaTrangThai { get; set; }

    public string? GhiChu { get; set; }

    public int? EmployeeId { get; set; }

    public virtual Employee? Employee { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual User User { get; set; } = null!;
}
