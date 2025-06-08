using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CuaHangVHT.Data;
using CuaHangVHT.Helper;
using CuaHangVHT.ViewModels;
using AutoMapper;
using X.PagedList.Extensions;
using Microsoft.AspNetCore.Authorization;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace CuaHangVHT.Controllers
{
    public class BlogPostsController : Controller
    {
        private readonly TuanStoreContext _context;
        private readonly IMapper _mapper;
        private readonly INotyfService _notyf;

        public BlogPostsController(TuanStoreContext context, IMapper mapper, INotyfService notyf)
        {
            _context = context;
            _mapper = mapper;
            _notyf = notyf;
        }

        // GET: BlogPosts
        public async Task<IActionResult> Index(int page = 1, int pageSize = 6)
        {
            var tuanStoreContext = _context.BlogPosts
                                    .Include(b => b.User)
                                    .OrderBy(b => b.IdBlogPost);
            // Phân trang dữ liệu
            var pagedResult = tuanStoreContext.ToPagedList(page, pageSize);
            return View(pagedResult); // Truyền pagedResult cho View
        }

        // GET: BlogPosts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogPost = await _context.BlogPosts
                .Include(b => b.User)
                .FirstOrDefaultAsync(m => m.IdBlogPost == id);
            if (blogPost == null)
            {
                return NotFound();
            }

            return View(blogPost);
        }

        [Authorize(Roles = "Admin, Manager, Staff")]
        // GET: BlogPosts/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId");
            return View();
        }

        // POST: BlogPosts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        [ValidateAntiForgeryToken]//****Lưu Ý
        [Authorize(Roles = "Admin, Manager, Staff")]
        [HttpPost]
        public IActionResult Create(BlogVM model, IFormFile? ImgBlog)
        {
            if (model.ImgBlog != null)
            {
                
                MyUtil.UploadHinh(ImgBlog, "images/blog");
            }
            var Blog = _mapper.Map<BlogPost>(model);

            Blog.ImageUrl = model.ImgBlog.FileName.ToString();
            Blog.CreatedAt = DateTime.Now;

            _context.Add(Blog);
            _context.SaveChangesAsync();
            _notyf.Success("Tạo Bài Viết Thành Công!");
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin, Manager, Staff")]
        // GET: BlogPosts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogPost = await _context.BlogPosts.FindAsync(id);
            if (blogPost == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", blogPost.UserId);
            return View(blogPost);
        }

        // POST: BlogPosts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin, Manager, Staff")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdBlogPost,Title,Content,CreatedAt,UserId,ImageUrl")] BlogPost blogPost)
        {
            if (id != blogPost.IdBlogPost)
            {
                return NotFound();
            }

            
                try
                {
                    _context.Update(blogPost);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BlogPostExists(blogPost.IdBlogPost))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", blogPost.UserId);
            return View(blogPost);
        }

        [Authorize(Roles = "Admin, Manager, Staff")]
        // GET: BlogPosts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogPost = await _context.BlogPosts
                .Include(b => b.User)
                .FirstOrDefaultAsync(m => m.IdBlogPost == id);
            if (blogPost == null)
            {
                return NotFound();
            }

            return View(blogPost);
        }

        // POST: BlogPosts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var blogPost = await _context.BlogPosts.FindAsync(id);
            if (blogPost != null)
            {
                _context.BlogPosts.Remove(blogPost);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BlogPostExists(int id)
        {
            return _context.BlogPosts.Any(e => e.IdBlogPost == id);
        }
    }
}
