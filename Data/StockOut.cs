using System;
using System.Collections.Generic;

namespace CuaHangVHT.Data;

public partial class StockOut
{
    public int StockOutId { get; set; }

    public int WarehouseId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public string? Reason { get; set; }

    public DateTime? DateOut { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual Warehouse Warehouse { get; set; } = null!;
}
