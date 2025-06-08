using Microsoft.AspNetCore.Mvc;

namespace CuaHangVHT.Controllers
{
    public class Test : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
