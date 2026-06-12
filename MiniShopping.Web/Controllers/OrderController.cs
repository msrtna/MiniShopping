using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniShopping.Web.Services.OrderServices;

namespace MiniShopping.Web.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IOrderService _service;

        public OrderController(IOrderService service)
        {
            _service = service;
        }

        // نمایش سفارش‌های کاربر
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Account");

            var orders = await _service.GetOrdersAsync(userId);

            return View(orders);
        }

        // ثبت سفارش از روی سبد خرید
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(userId))
                    return RedirectToAction("Login", "Account");

                await _service.CheckoutAsync(userId);

                TempData["Success"] = "Order placed successfully.";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;

                return RedirectToAction("Index", "Basket");
            }
        }

        // جزئیات سفارش
        public async Task<IActionResult> Details(int id)
        {
            var details = await _service.GetOrderDetailsAsync(id);

            return View(details);
        }
    }
}
