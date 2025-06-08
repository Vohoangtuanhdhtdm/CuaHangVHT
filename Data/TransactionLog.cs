using System;
using System.Collections.Generic;

namespace CuaHangVHT.Data;

public partial class TransactionLog
{
    public int TransactionId { get; set; }

    public int ProductId { get; set; }

    public int WarehouseId { get; set; }

    public string TransactionType { get; set; } = null!;

    public int Quantity { get; set; }

    public DateTime? Date { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual Warehouse Warehouse { get; set; } = null!;
}
