using System;
using System.Collections.Generic;

namespace CuaHangVHT.Data;

public partial class Payment
{
    public int PaymentId { get; set; }

    public int OrderId { get; set; }

    public string PaymentMethod { get; set; } = null!;

    public string? PaymentStatus { get; set; }

    public DateTime? PaymentDate { get; set; }

    public virtual Order Order { get; set; } = null!;
}
