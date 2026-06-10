using MiniShopping.Web.Models;

namespace MiniShopping.Web.Repositories
{
    public interface IOrderItemRepository
    {
        Task<List<OrderItem>> GetByOrderIdAsync(int orderId);
        Task AddRangeAsync(List<OrderItem> orderItems);
    }
}
