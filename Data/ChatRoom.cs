using System;
using System.Collections.Generic;

namespace CuaHangVHT.Data;

public partial class ChatRoom
{
    public int ChatroomId { get; set; }

    public string Name { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();
}
