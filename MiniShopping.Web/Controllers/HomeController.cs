using Microsoft.AspNetCore.Mvc;

namespace MiniShopping.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
