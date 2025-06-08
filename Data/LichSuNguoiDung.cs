using System;
using System.Collections.Generic;

namespace CuaHangVHT.Data;

public partial class LichSuNguoiDung
{
    public int Id { get; set; }

    public int IdNguoiDung { get; set; }

    public int IdlichSu { get; set; }

    public DateTime? DateUsed { get; set; }

    public virtual NguoiDung IdNguoiDungNavigation { get; set; } = null!;

    public virtual LichSuKetQua IdlichSuNavigation { get; set; } = null!;
}
