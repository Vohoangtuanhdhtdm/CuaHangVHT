using CuaHangVHT.Data;
using System.ComponentModel.DataAnnotations;

namespace CuaHangVHT.ViewModels
{
    public class BlogVM
    {
        public int IdBlogPost { get; set; }

        public string Title { get; set; } = null!;

        public string Content { get; set; } = null!;

        public DateTime? CreatedAt { get; set; }
        public int UserId { get; set; }

        [Display(Name = "Ảnh đại diện")]
        public IFormFile? ImgBlog { get; set; }
    }
}