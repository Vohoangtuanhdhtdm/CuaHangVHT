using System;
using System.Collections.Generic;

namespace CuaHangVHT.Data;

public partial class Connection
{
    public string ConnectionId { get; set; } = null!;

    public int UserId { get; set; }

    public DateTime? ConnectedAt { get; set; }

    public DateTime? DisconnectedAt { get; set; }

    public virtual User User { get; set; } = null!;
}
