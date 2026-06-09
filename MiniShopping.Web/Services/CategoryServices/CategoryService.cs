using MiniShopping.Web.DTOs.CategoryDtos;
using MiniShopping.Web.Models;
using MiniShopping.Web.UnitOfWorks;

namespace MiniShopping.Web.Services.CategoryServices
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _uow;
        public CategoryService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<List<CategoryDto>> GetAllAsync()
        {
            var category = await _uow.Category.GetAllAsync();
            return category.Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name
            }).ToList();
        }
        public async Task<CategoryDto> GetByIdAsync(int id)
        {
            var category = await _uow.Category.GetByIdAsync(id);
            if (category == null)
                throw new Exception("Category not found");
            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name
            };
        }
        public async Task<string> AddAsync(CreateCategoryDto dto)
        {
            var category = new Category()
            {
                Name = dto.Name
            };
            await _uow.Category.AddAsync(category);
            await _uow.SaveAsync();

            return "Category created successfuly";
        }
        public async Task<string> UpdateAsync(UpdateCategoryDto dto)
        {
            var category = await _uow.Category.GetByIdAsync(dto.Id);
            if (category == null)
                throw new Exception("Category not found");
            category.Name = dto.Name;

            await _uow.Category.UpdateAsync(category);
            await _uow.SaveAsync();

            return "Category updated successfuly";
        }
        public async Task<string> DeleteAsync(int id)
        {
            var category = await _uow.Category.GetByIdAsync(id);

            if (category == null)
                throw new Exception("Category not found");

            await _uow.Category.DeleteAsync(id);
            await _uow.SaveAsync();

            return "Category deleted successfully";
        }
    }
}
