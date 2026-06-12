using MiniShopping.Web.DTOs.OrderDtos;

namespace MiniShopping.Web.Services.OrderServices
{
    public interface IOrderService
    {
        Task<string> CheckoutAsync(string userId);
        Task<List<OrderDto>> GetOrdersAsync(string userId);
        Task<List<OrderDetailDto>> GetOrderDetailsAsync(int orderId);
    }
}
