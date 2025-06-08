using Microsoft.AspNetCore.Mvc;

namespace CuaHangVHT.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Route("/404")]
        public IActionResult PageError()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Blog()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Test()
        {
            return View();
        }

    }
}
