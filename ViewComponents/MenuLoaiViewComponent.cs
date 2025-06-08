using CuaHangVHT.Data;
using CuaHangVHT.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CuaHangVHT.ViewComponents
{
    public class MenuLoaiViewComponent : ViewComponent
    {
        private readonly TuanStoreContext db;

        public MenuLoaiViewComponent(TuanStoreContext context) => db = context;
        // chỉ có 1 Action
        // Action chuyển qua View thì cần có 1 cái View Model
        public IViewComponentResult Invoke()
        {
            var data = db.Categories.Select(c => new MenuLoaiVM { TenLoai= c.Name, MaLoai = c.CategoryId, SoLuong = c.Products.Count }).OrderBy(p => p.TenLoai);

            return View(data);
        }
    }
}
