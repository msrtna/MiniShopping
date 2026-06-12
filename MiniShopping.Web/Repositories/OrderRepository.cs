using Microsoft.EntityFrameworkCore;
using MiniShopping.Web.Data;
using MiniShopping.Web.Models;

namespace MiniShopping.Web.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _context;

        public OrderRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Order>> GetOrdersAsync(string userId)
        {
            return await _context.Orders.Include(o=> o.OrderItems).Where(o=> o.UserId == userId).ToListAsync();
        }
        public async Task<Order?> GetByIdAsync(int id)
        {
            return await _context.Orders.Include(o=> o.OrderItems).ThenInclude(p=> p.Product).FirstOrDefaultAsync(o=> o.Id == id);
        }
        public async Task AddAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
        }
        public async Task<List<Order>> GetAllAsync()
        {
            return await _context.Orders.Include(o => o.User).OrderByDescending(o => o.OrderDate).ToListAsync();
        }
        public Task UpdateAsync(Order order)
        {
            _context.Orders.Update(order);
            return Task.CompletedTask;
        }
    }
}
