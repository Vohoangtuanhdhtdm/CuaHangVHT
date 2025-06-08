using System;
using System.Collections.Generic;

namespace CuaHangVHT.Data;

public partial class Warehouse
{
    public int WarehouseId { get; set; }

    public string Name { get; set; } = null!;

    public string? Location { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<StockIn> StockIns { get; set; } = new List<StockIn>();

    public virtual ICollection<StockOut> StockOuts { get; set; } = new List<StockOut>();

    public virtual ICollection<Stock> Stocks { get; set; } = new List<Stock>();

    public virtual ICollection<TransactionLog> TransactionLogs { get; set; } = new List<TransactionLog>();
}
