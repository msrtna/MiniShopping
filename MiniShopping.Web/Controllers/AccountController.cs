using Microsoft.AspNetCore.Mvc;
using MiniShopping.Web.DTOs.AccountDtos;
using MiniShopping.Web.Services.AccountServices;

namespace MiniShopping.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var result = await _accountService.RegisterAsync(dto);

            if (result != "User created successfully")
            {
                ModelState.AddModelError("", result);
                return View(dto);
            }

            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var result = await _accountService.LoginAsync(dto);

            if (result != "Login successful")
            {
                ModelState.AddModelError("", result);
                return View(dto);
            }

            return RedirectToAction("Home");
        }
    }
}
