using MiniShopping.Web.DTOs.BasketItemDtos;
using MiniShopping.Web.Models;

namespace MiniShopping.Web.Services.BasketItemServices
{
    public interface IBasketService
    {
        Task<List<BasketItemDto>> GetBasketItemsAsync(string userId);
        Task<string> AddToBasketAsync(CreateBasketItemDto dto);
        Task<string> UpdateQuantity(UpdateBasketItemDto dto);
        Task<string> DeleteBasketAsync(int  id, string userId);
        Task<BasketItemDto?> GetByUserAndProductAsync(string userId, int productId);
    }
}
