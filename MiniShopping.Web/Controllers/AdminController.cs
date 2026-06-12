using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniShopping.Web.Services.OrderServices;
using MiniShopping.Web.Services.UserServices;

namespace MiniShopping.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IUserService _userService;
        private readonly IOrderService _orderService;

        public AdminController(IUserService userService, IOrderService orderService)
        {
            _userService = userService;
            _orderService = orderService;
        }

        public async Task<IActionResult> Users()
        {
            var users = await _userService.GetAllUsersAsync();

            return View(users);
        }

        public async Task<IActionResult> Orders()
        {
            var orders = await _orderService.GetAllOrdersAsync();

            return View(orders);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateOrderStatus(int orderId,string status)
        {
            try
            {
                await _orderService.UpdateStatusAsync(orderId, status);

                TempData["Success"] = "Status updated.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Orders));
        }
    }
}
