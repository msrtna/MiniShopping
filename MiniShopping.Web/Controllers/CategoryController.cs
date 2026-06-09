using Microsoft.AspNetCore.Mvc;
using MiniShopping.Web.DTOs.CategoryDtos;
using MiniShopping.Web.Services.CategoryServices;

namespace MiniShopping.Web.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _service;
        public CategoryController(ICategoryService service)
        {
            _service = service;
        }

        // Index ------------>>>
        public async Task<ActionResult> Index()
        {
            var categories = await _service.GetAllAsync();
            return View(categories);
        }

        // Create ------------>>>
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryDto dto)
        {
            await _service.AddAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        // Update ------------>>>
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _service.GetByIdAsync(id);
            var dto = new UpdateCategoryDto
            {
                Id = category.Id,
                Name = category.Name
            };
            return View(dto);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(UpdateCategoryDto dto)
        {
            await _service.UpdateAsync(dto);
            return RedirectToAction(nameof(Index));
        }
    }
}
