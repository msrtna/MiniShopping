using AutoMapper;
using MiniShopping.Web.DTOs.BasketItemDtos;
using MiniShopping.Web.DTOs.CategoryDtos;
using MiniShopping.Web.DTOs.OrderDtos;
using MiniShopping.Web.DTOs.ProductDtos;
using MiniShopping.Web.DTOs.UserDtos;
using MiniShopping.Web.Models;

namespace MiniShopping.Web.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Product
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.CategoryName,
                    opt => opt.MapFrom(src =>
                        src.Category != null
                            ? src.Category.Name
                            : string.Empty));
            CreateMap<CreateProductDto, Product>();
            CreateMap<Product, UpdateProductDto>().ReverseMap();

            // Category
            CreateMap<Category, CategoryDto>();
            CreateMap<CreateCategoryDto, Category>();
            CreateMap<Category, UpdateCategoryDto>().ReverseMap();

            // BasketItem
            CreateMap<BasketItem, BasketItemDto>()
                .ForMember(dest => dest.ProductName,
                    opt => opt.MapFrom(src => src.Product.Name))

                .ForMember(dest => dest.UnitPrice,
                    opt => opt.MapFrom(src => src.Product.Price))

                .ForMember(dest => dest.TotalPrice,
                    opt => opt.MapFrom(src =>
                        src.Quantity * src.Product.Price));

            CreateMap<CreateBasketItemDto, BasketItem>();
            CreateMap<UpdateBasketItemDto, BasketItem>();

            // Order
            CreateMap<Order, OrderDto>();
            CreateMap<OrderItem, OrderDetailDto>()
                .ForMember(dest => dest.ProductName,
                    opt => opt.MapFrom(src =>
                        src.Product != null
                            ? src.Product.Name
                            : string.Empty));
            CreateMap<Order, AdminOrderDto>()
                .ForMember(dest => dest.UserEmail,
                    opt => opt.MapFrom(src =>
                        src.User != null
                            ? src.User.Email
                            : string.Empty));

            // User
            CreateMap<ApplicationUser, UserDto>();
        }
    }
}
