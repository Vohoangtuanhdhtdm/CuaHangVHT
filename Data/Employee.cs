using System;
using System.Collections.Generic;

namespace CuaHangVHT.Data;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Email { get; set; }

    public int? PositionId { get; set; }

    public int? UserId { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual Position? Position { get; set; }

    public virtual User? User { get; set; }
}
