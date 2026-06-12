using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MiniShopping.Web.DTOs.ProductDtos;
using MiniShopping.Web.Services.CategoryServices;
using MiniShopping.Web.Services.ProductServices;

namespace MiniShopping.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _service;
        private readonly ICategoryService _categoryService;
        public ProductController(IProductService service, ICategoryService categoryService)
        {
            _service = service;
            _categoryService = categoryService;
        }

        // Index ------------>>>
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] ProductQueryDto query)
        {
            var result = await _service.GetProductsPagedAsync(query);

            return View(result);
        }
        //public async Task<ActionResult> Index()
        //{
        //    var products = await _service.GetAllAsync();
        //    return View(products);
        //}

        // Create ------------>>>
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var categories = await _categoryService.GetAllAsync();

            ViewBag.Categories = categories
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).ToList();

            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CreateProductDto dto)
        {
            await _service.AddAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        // Update ------------>>>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _service.GetByIdAsync(id);
            var dto = new UpdateProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Quantity = product.Quantity,
                CategoryId = product.CategoryId
            };
            var categories = await _categoryService.GetAllAsync();

            ViewBag.Categories = categories
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).ToList();

            return View(dto);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(UpdateProductDto dto)
        {
            await _service.UpdateAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        // Delete ------------>>>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _service.GetByIdAsync(id);
            var dto = new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Quantity = product.Quantity,
                CategoryId = product.CategoryId,
                CategoryName = product.CategoryName
            };
            return View(dto);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(ProductDto dto)
        {
            await _service.DeleteAsync(dto.Id);
            return RedirectToAction(nameof(Index));
        }
    }
}
