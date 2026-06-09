
using MiniShopping.Web.DTOs.ProductDtos;

namespace MiniShopping.Web.Services.ProductServices
{
    public interface IProductService
    {
        Task<List<ProductDto>> GetAllAsync();
        Task<ProductDto> GetByIdAsync(int id);
        Task<string> AddAsync(CreateProductDto dto);
        Task<string> UpdateAsync(UpdateProductDto dto);
        Task<string> DeleteAsync(int id);
    }
}
