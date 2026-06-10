using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniShopping.Web.DTOs.BasketItemDtos;
using MiniShopping.Web.Services.BasketItemServices;

namespace MiniShopping.Web.Controllers
{
    [Authorize]
    public class BasketController : Controller
    {
        private readonly IBasketService _basketService;

        public BasketController(IBasketService basketService)
        {
            _basketService = basketService;
        }

        // نمایش سبد خرید کاربر
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var basketItems = await _basketService.GetBasketItemsAsync(userId!);

            return View(basketItems);
        }

        // افزودن محصول به سبد
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(CreateBasketItemDto dto)
        {
            dto.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            await _basketService.AddToBasketAsync(dto);

            return RedirectToAction(nameof(Index));
        }

        // فرم ویرایش تعداد
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var userId =
                User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            var basketItems =
                await _basketService.GetBasketItemsAsync(userId);

            var basket =
                basketItems.FirstOrDefault(x => x.Id == id);

            if (basket == null)
                return NotFound();

            var dto = new UpdateBasketItemDto
            {
                Id = basket.Id,
                Quantity = basket.Quantity
            };

            return View(dto);
        }

        // ثبت ویرایش تعداد
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateBasketItemDto dto)
        {
            await _basketService.UpdateQuantity(dto);

            return RedirectToAction(nameof(Index));
        }

        // حذف آیتم
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var userId =
                User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            await _basketService.DeleteBasketAsync(id, userId);

            return RedirectToAction(nameof(Index));
        }
    }
}