using CuaHangVHT.Helper;
using CuaHangVHT.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CuaHangVHT.ViewComponents
{
    public class ModalCartViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var cart = HttpContext.Session.Get<List<CartItemVM>>(MySetting.CART_KEY) ?? new List<CartItemVM>();
			//return View("ModalCartPanel", new CartCount
			//{
			//    Quantity = cart.Sum(p => p.SoLuong),
			//    Total = cart.Sum(p => p.ThanhTien)
			//});
			return View("ModalCartPanel", cart);
		}
    }
}
