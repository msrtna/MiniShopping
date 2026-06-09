using MiniShopping.Web.DTOs.ProductDtos;
using MiniShopping.Web.UnitOfWorks;
using MiniShopping.Web.Models;


namespace MiniShopping.Web.Services.ProductServices
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _uow;
        public ProductService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<List<ProductDto>> GetAllAsync()
        {
            var product = await _uow.Product.GetAllAsync();
            return product.Select(p=> new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                Quantity = p.Quantity,
                CategoryId = p.CategoryId,
                CategoryName = p.Category?.Name?? ""
            }).ToList();
        }
        public async Task<ProductDto> GetByIdAsync(int id)
        {
            var product = await _uow.Product.GetByIdAsync(id);
            if (product == null)
                throw new Exception("Product not found");
            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Quantity = product.Quantity,
                CategoryId = product.CategoryId,
                CategoryName = product.Category?.Name?? ""
            };
        }
        public async Task<string> AddAsync(CreateProductDto dto)
        {
            var product = new Product()
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                Quantity = dto.Quantity,
                CategoryId = dto.CategoryId
            };
            await _uow.Product.AddAsync(product);
            await _uow.SaveAsync();

            return "Product created successfuly";
        }
        public async Task<string> UpdateAsync(UpdateProductDto dto)
        {
            var product = await _uow.Product.GetByIdAsync(dto.Id);
            if (product == null)
                throw new Exception("Product not found");
            product.Name = dto.Name;
            product.Description = dto.Description;
            product.Price = dto.Price;
            product.Quantity = dto.Quantity;
            product.CategoryId = dto.CategoryId;

            await _uow.Product.UpdateAsync(product);
            await _uow.SaveAsync();

            return "Product updated successfuly";
        }
        public async Task<string> DeleteAsync(int id)
        {
            var product = await _uow.Product.GetByIdAsync(id);
            if (product == null)
                throw new Exception("Product not found");
            await _uow.Product.DeleteAsync(id);
            await _uow.SaveAsync();

            return "Product deleted successfuly";
        }
    }
}
