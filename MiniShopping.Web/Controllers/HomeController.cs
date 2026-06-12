using Microsoft.AspNetCore.Mvc;
using MiniShopping.Web.Models;

namespace MiniShopping.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error(string message)
        {
            return View(new ErrorViewModel
            {
                Message = message
            });
        }
    }
}
