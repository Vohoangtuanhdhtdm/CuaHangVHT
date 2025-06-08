using System;
using System.Collections.Generic;

namespace CuaHangVHT.Data;

public partial class LichSuKetQua
{
    public int IdlichSu { get; set; }

    public int IdPtbac2 { get; set; }

    public decimal? Root1 { get; set; }

    public decimal? Root2 { get; set; }

    public DateTime? DateCalculated { get; set; }

    public virtual Ptbac2 IdPtbac2Navigation { get; set; } = null!;

    public virtual ICollection<LichSuNguoiDung> LichSuNguoiDungs { get; set; } = new List<LichSuNguoiDung>();
}
