using System;
using System.Collections.Generic;

namespace CuaHangVHT.Data;

public partial class OrderDetail
{
    public int OrderDetailId { get; set; }

    public int OrderId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public decimal Price { get; set; }

    public string? Discount { get; set; }

    public int? ProductSizeId { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;

    public virtual ProductSize? ProductSize { get; set; }
}
