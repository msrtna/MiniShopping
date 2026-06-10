using MiniShopping.Web.Models;

namespace MiniShopping.Web.Repositories
{
    public interface IBasketRepository
    {
        Task<List<BasketItem>> GetBasketItemsAsync(string userId);
        Task<BasketItem?> GetByIdAsync(int  id);
        Task AddToBasketAsync(BasketItem item);
        Task UpdateQuantityAsync(BasketItem item);
        Task DeleteBasketAsync(int id, string userId);
        Task<BasketItem?> GetByUserAndProductAsync(string userId, int productId);
    }
}
