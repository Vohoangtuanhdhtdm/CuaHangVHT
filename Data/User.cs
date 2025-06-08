using System;
using System.Collections.Generic;

namespace CuaHangVHT.Data;

public partial class User
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? FullName { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Address { get; set; }

    public string? Role { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? RandomKey { get; set; }

    public string? ImgAvater { get; set; }

    public string? ResetPasswordToken { get; set; }

    public DateTime? TokenExpiration { get; set; }

    public virtual ICollection<BlogPost> BlogPosts { get; set; } = new List<BlogPost>();

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
}
