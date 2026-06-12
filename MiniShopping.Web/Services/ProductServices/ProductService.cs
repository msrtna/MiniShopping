using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MiniShopping.Web.DTOs.ProductDtos;
using MiniShopping.Web.Models;
using MiniShopping.Web.UnitOfWorks;


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
        public async Task<PagedResult<ProductDto>> GetProductsPagedAsync(ProductQueryDto query)
        {
            var productsQuery = await _uow.Product.GetQueryableAsync();

            // SEARCH
            if (!string.IsNullOrEmpty(query.Search))
            {
                productsQuery = productsQuery
                    .Where(p => p.Name.Contains(query.Search));
            }

            // FILTER
            if (query.CategoryId.HasValue)
            {
                productsQuery = productsQuery
                    .Where(p => p.CategoryId == query.CategoryId);
            }

            // SORT
            productsQuery = query.Sort switch
            {
                "price_asc" => productsQuery.OrderBy(p => p.Price),
                "price_desc" => productsQuery.OrderByDescending(p => p.Price),
                _ => productsQuery.OrderByDescending(p => p.Id)
            };

            var totalCount = await productsQuery.CountAsync();

            // PAGINATION
            var products = await productsQuery
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();

            return new PagedResult<ProductDto>
            {
                Items = _mapper.Map<List<ProductDto>>(products),
                TotalCount = totalCount,
                Page = query.Page,
                PageSize = query.PageSize
            };
        }
    }
}
