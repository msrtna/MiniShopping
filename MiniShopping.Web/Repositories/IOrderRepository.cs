using MiniShopping.Web.Models;

namespace MiniShopping.Web.Repositories
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetOrdersAsync(string userId);
        Task<Order?> GetByIdAsync(int id);
        Task AddAsync(Order order);
    }
}
