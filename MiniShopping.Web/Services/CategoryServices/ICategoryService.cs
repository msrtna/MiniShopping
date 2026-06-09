using MiniShopping.Web.DTOs.CategoryDtos;

namespace MiniShopping.Web.Services.CategoryServices
{
    public interface ICategoryService
    {
        Task<List<CategoryDto>> GetAllAsync();
        Task<CategoryDto> GetByIdAsync(int id);
        Task<string> AddAsync(CreateCategoryDto dto);
        Task<string> UpdateAsync(UpdateCategoryDto dto);
        Task<string> DeleteAsync(int id);
    }
}
