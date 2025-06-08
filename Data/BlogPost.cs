using System;
using System.Collections.Generic;

namespace CuaHangVHT.Data;

public partial class BlogPost
{
    public int IdBlogPost { get; set; }

    public string Title { get; set; } = null!;

    public string Content { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public int UserId { get; set; }

    public string? ImageUrl { get; set; }

    public virtual User User { get; set; } = null!;
}
