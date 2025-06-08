using System;
using System.Collections.Generic;

namespace CuaHangVHT.Data;

public partial class OrderDetailPromotion
{
    public int OrderDetailPromotionId { get; set; }

    public int PromotionId { get; set; }

    public int? ProductId { get; set; }

    public virtual Product? Product { get; set; }

    public virtual Promotion Promotion { get; set; } = null!;
}
