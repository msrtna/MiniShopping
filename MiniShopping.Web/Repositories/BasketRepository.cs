using Microsoft.EntityFrameworkCore;
using MiniShopping.Web.Data;
using MiniShopping.Web.Models;

namespace MiniShopping.Web.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly AppDbContext _context;
        public BasketRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<BasketItem>> GetBasketItemsAsync(string userId)
        {
            return await _context.BasketItems.Include(u=> u.Product).Where(u=> u.UserId == userId).ToListAsync();
        }
        public async Task<BasketItem?> GetByIdAsync(int id)
        {
            return await _context.BasketItems.Include(b=> b.Product).FirstOrDefaultAsync(b=> b.Id == id);
        }
        public async Task AddToBasketAsync(BasketItem item)
        {
            await _context.BasketItems.AddAsync(item);
        }
        public Task UpdateQuantityAsync(BasketItem item)
        {
            _context.BasketItems.Update(item);
            return Task.CompletedTask;
        }
        public async Task DeleteBasketAsync(int id, string userId)
        {
            var basket = await _context.BasketItems.FirstOrDefaultAsync(d=> d.Id == id && d.UserId == userId);
            if (basket != null)
                _context.BasketItems.Remove(basket);
        }
        public async Task<BasketItem?> GetByUserAndProductAsync(string userId, int productId)
        {
            return await _context.BasketItems.Include(b => b.Product)
                                                            .FirstOrDefaultAsync(b => b.UserId == userId && b.ProductId == productId);
        }
    }
}
