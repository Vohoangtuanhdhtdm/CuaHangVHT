using System;
using System.Collections.Generic;

namespace CuaHangVHT.Data;

public partial class ProductSize
{
    public int ProductSizeId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public string SizeName { get; set; } = null!;

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual Product Product { get; set; } = null!;
}
