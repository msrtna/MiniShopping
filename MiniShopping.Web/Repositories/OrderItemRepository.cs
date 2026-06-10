using Microsoft.EntityFrameworkCore;
using MiniShopping.Web.Data;
using MiniShopping.Web.Models;

namespace MiniShopping.Web.Repositories
{
    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly AppDbContext _context;

        public OrderItemRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<OrderItem>> GetByOrderIdAsync(int orderId)
        {
            return await _context.OrderItems.Include(o=> o.Product).Where(o=> o.OrderId == orderId).ToListAsync();
        }
        public async Task AddRangeAsync(List<OrderItem> orderItems)
        {
            await _context.OrderItems.AddRangeAsync(orderItems);
        }
    }
}
