using System;
using System.Collections.Generic;

namespace CuaHangVHT.Data;

public partial class Stock
{
    public int StockId { get; set; }

    public int ProductId { get; set; }

    public int WarehouseId { get; set; }

    public int Quantity { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual Warehouse Warehouse { get; set; } = null!;
}
