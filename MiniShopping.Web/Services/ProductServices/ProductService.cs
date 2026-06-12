using MiniShopping.Web.DTOs.ProductDtos;
using MiniShopping.Web.UnitOfWorks;
using MiniShopping.Web.Models;
using AutoMapper;


namespace MiniShopping.Web.Services.ProductServices
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        public ProductService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<List<ProductDto>> GetAllAsync()
        {
            var product = await _uow.Product.GetAllAsync();
            return _mapper.Map<List<ProductDto>>(product);
        }
        public async Task<ProductDto> GetByIdAsync(int id)
        {
            var product = await _uow.Product.GetByIdAsync(id);
            if (product == null)
                throw new Exception("Product not found");
            return _mapper.Map<ProductDto>(product);
        }
        public async Task<string> AddAsync(CreateProductDto dto)
        {
            var product = _mapper.Map<Product>(dto);

            await _uow.Product.AddAsync(product);
            await _uow.SaveAsync();

            return "Product created successfuly";
        }
        public async Task<string> UpdateAsync(UpdateProductDto dto)
        {
            var product = await _uow.Product.GetByIdAsync(dto.Id);
            if (product == null)
                throw new Exception("Product not found");

            _mapper.Map(dto, product);

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
