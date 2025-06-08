using System;
using System.Collections.Generic;

namespace CuaHangVHT.Data;

public partial class Promotion
{
    public int PromotionId { get; set; }

    public string PromotionName { get; set; } = null!;

    public decimal DiscountPercent { get; set; }

    public DateTime StartPrDate { get; set; }

    public DateTime EndPrDate { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<OrderDetailPromotion> OrderDetailPromotions { get; set; } = new List<OrderDetailPromotion>();
}
