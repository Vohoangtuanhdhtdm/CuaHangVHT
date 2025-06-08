using CuaHangVHT.Data;
using CuaHangVHT.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using X.PagedList.Extensions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace CuaHangVHT.Controllers
{
    public class HangHoaController : Controller
    {
        private readonly TuanStoreContext db;

        public HangHoaController(TuanStoreContext context) { db = context; }
        public IActionResult Index(int? loai , int page = 1, int pageSize = 9)
        {
            var hangHoa = db.Products.AsQueryable();
            if (loai.HasValue)
            {
                //Loc loai
                hangHoa = hangHoa.Where(p => p.CategoryId == loai.Value).Include(p => p.OrderDetailPromotions).ThenInclude(pr => pr.Promotion); // Thêm Khuyến mãi tại đây
            }

       

            var result = hangHoa.Select(p => new HangHoaVM
            {
                MaHh = p.ProductId,
                TenHH = p.Name,
                DonGia = p.Price,
                Hinh = p.ImageUrl ?? "",
                MoTaNgan = p.Description ?? "",
                PhanTramGiamGia = p.OrderDetailPromotions.SingleOrDefault(od => od.ProductId == p.ProductId).Promotion.DiscountPercent,
            });

            // Phân trang dữ liệu
            var pagedResult = result.ToPagedList(page, pageSize);

            return View(pagedResult);
        }
        public IActionResult Search(string? tenSP) 
        {
            var hangHoa = db.Products.AsQueryable();

            if (tenSP != null)
            {
                hangHoa = hangHoa.Where(p => p.Name.Contains(tenSP)).Include(p => p.OrderDetailPromotions).ThenInclude(pr => pr.Promotion); // Thêm Khuyến mãi tại đây;
            }

            var result = hangHoa.Select(p => new HangHoaVM
            {
                MaHh = p.ProductId,
                TenHH = p.Name,
                DonGia = p.Price,
                Hinh = p.ImageUrl ?? "",
                MoTaNgan = p.Description ?? "",
                PhanTramGiamGia = p.OrderDetailPromotions.SingleOrDefault(od => od.ProductId == p.ProductId).Promotion.DiscountPercent,
            });
            return View(result);
        }

        [Authorize]
        public IActionResult Detail(int id)
        {
            var data = db.Products
                .Include(p => p.Category)
                .Include(p => p.Stocks)
                .SingleOrDefault(p => p.ProductId == id);
            if (data == null)
            {
                TempData["Message"] = $"Không tìm thấy sản phẩm có mã {id}";
                return Redirect("/404");
            }
            // Lấy stock đầu tiên hoặc theo điều kiện mong muốn (ví dụ: WarehouseId)
            var stock = data.Stocks.FirstOrDefault(s => s.ProductId ==id); // Hoặc: data.Stocks.FirstOrDefault(s => s.WarehouseId == 1);
            var size = db.ProductSizes.Where(ps => ps.ProductId == data.ProductId).Select(ps => new ProductSizeVM {Size = ps.SizeName, Quantity= ps.Quantity, IdSize = ps.ProductSizeId }).ToList();
            var result = new ChiTietHangHoaVM
            {
                MaHh = data.ProductId,
                TenHH = data.Name,
                DonGia = data.Price,
                MoTaNgan = data.Description,
                Hinh = data.ImageUrl,
                TenLoai = data.Category.Name,
                soLuong = data.Stock,
                Sizes = size,
               
            };
            return View(result);
        }
        

       
    }
}
