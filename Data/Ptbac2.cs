using System;
using System.Collections.Generic;

namespace CuaHangVHT.Data;

public partial class Ptbac2
{
    public int IdPtbac2 { get; set; }

    public decimal A { get; set; }

    public decimal B { get; set; }

    public decimal C { get; set; }

    public DateTime? DateCreated { get; set; }

    public virtual ICollection<LichSuKetQua> LichSuKetQuas { get; set; } = new List<LichSuKetQua>();
}
