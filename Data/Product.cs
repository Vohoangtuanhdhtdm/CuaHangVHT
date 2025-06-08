using System;
using System.Collections.Generic;

namespace CuaHangVHT.Data;

public partial class Product
{
    public int ProductId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public int Stock { get; set; }

    public int? CategoryId { get; set; }

    public string? ImageUrl { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual Category? Category { get; set; }

    public virtual ICollection<OrderDetailPromotion> OrderDetailPromotions { get; set; } = new List<OrderDetailPromotion>();

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<ProductSize> ProductSizes { get; set; } = new List<ProductSize>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual ICollection<StockIn> StockIns { get; set; } = new List<StockIn>();

    public virtual ICollection<StockOut> StockOuts { get; set; } = new List<StockOut>();

    public virtual ICollection<Stock> Stocks { get; set; } = new List<Stock>();

    public virtual ICollection<TransactionLog> TransactionLogs { get; set; } = new List<TransactionLog>();
}
