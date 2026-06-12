using AutoMapper;
using MiniShopping.Web.DTOs.CategoryDtos;
using MiniShopping.Web.Models;
using MiniShopping.Web.UnitOfWorks;

namespace MiniShopping.Web.Services.CategoryServices
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public CategoryService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<List<CategoryDto>> GetAllAsync()
        {
            var category = await _uow.Category.GetAllAsync();
            return _mapper.Map<List<CategoryDto>>(category);
        }
        public async Task<CategoryDto> GetByIdAsync(int id)
        {
            var category = await _uow.Category.GetByIdAsync(id);
            if (category == null)
                throw new Exception("Category not found");
            return _mapper.Map<CategoryDto>(category);
        }
        public async Task<string> AddAsync(CreateCategoryDto dto)
        {
            var category = _mapper.Map<Category>(dto);
            await _uow.Category.AddAsync(category);
            await _uow.SaveAsync();

            return "Category created successfuly";
        }
        public async Task<string> UpdateAsync(UpdateCategoryDto dto)
        {
            var category = await _uow.Category.GetByIdAsync(dto.Id);
            if (category == null)
                throw new Exception("Category not found");
            _mapper.Map(dto, category);

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
