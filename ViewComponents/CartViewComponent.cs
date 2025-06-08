using CuaHangVHT.Helper;
using CuaHangVHT.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CuaHangVHT.ViewComponents
{
    public class CartViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
           var cart = HttpContext.Session.Get<List<CartItemVM>>(MySetting.CART_KEY) ?? new List<CartItemVM>();
            return View("CartPanel", new CartCount
            {
                Quantity = cart.Sum(p => p.SoLuong),
                Total = cart.Sum(p => p.ThanhTien)
            });
        }
    }
}
