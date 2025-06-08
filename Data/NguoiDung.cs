using System;
using System.Collections.Generic;

namespace CuaHangVHT.Data;

public partial class NguoiDung
{
    public int IdNguoiDung { get; set; }

    public string UserName { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string Email { get; set; } = null!;

    public DateTime? DateCreated { get; set; }

    public virtual ICollection<LichSuNguoiDung> LichSuNguoiDungs { get; set; } = new List<LichSuNguoiDung>();
}
