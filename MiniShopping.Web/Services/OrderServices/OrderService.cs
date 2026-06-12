using AutoMapper;
using MiniShopping.Web.Data;
using MiniShopping.Web.DTOs.OrderDtos;
using MiniShopping.Web.Models;
using MiniShopping.Web.UnitOfWorks;

namespace MiniShopping.Web.Services.OrderServices
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _uow;
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public OrderService(IUnitOfWork uow, AppDbContext context, IMapper mapper)
        {
            _uow = uow;
            _context = context;
            _mapper = mapper;
        }

        public async Task<string> CheckoutAsync(string userId)
        {
            await using var transaction =
                await _context.Database.BeginTransactionAsync();

            try
            {
                // گرفتن سبد خرید
                var basketItems =
                    await _uow.Basket.GetBasketItemsAsync(userId);

                if (!basketItems.Any())
                    throw new Exception("Basket is empty.");

                var productIds = basketItems.Select(x => x.ProductId).ToList();
                var products = await _uow.Product.GetAllAsync();

                var productDict = products
                    .Where(p => productIds.Contains(p.Id))
                    .ToDictionary(p => p.Id);

                decimal totalAmount = 0;

                // بررسی موجودی محصولات
                foreach (var item in basketItems)
                {
                    var product = productDict[item.ProductId];

                    if (product.Quantity < item.Quantity)
                        throw new Exception($"Not enough stock for {product.Name}.");

                    totalAmount += item.Quantity * product.Price;
                }

                // ساخت سفارش
                var order = new Order
                {
                    UserId = userId,
                    OrderDate = DateTime.Now,
                    Status = "Pending",
                    TotalAmount = totalAmount
                };

                await _uow.Order.AddAsync(order);

                // لازم است Order ذخیره شود تا Id بگیرد
                await _uow.SaveAsync();

                // ساخت آیتم‌های سفارش
                var orderItems = new List<OrderItem>();

                foreach (var item in basketItems)
                {
                    var product = productDict[item.ProductId];

                    orderItems.Add(new OrderItem
                    {
                        OrderId = order.Id,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        UnitPrice = product.Price
                    });

                    product.Quantity -= item.Quantity;

                    await _uow.Product.UpdateAsync(product);

                    await _uow.Basket.DeleteBasketAsync(item.Id, userId);
                }

                await _uow.OrderItem.AddRangeAsync(orderItems);

                await _uow.SaveAsync();

                await transaction.CommitAsync();

                return "Checkout completed successfully.";
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        public async Task<List<OrderDetailDto>> GetOrderDetailsAsync(int orderId)
        {
            var items = await _uow.OrderItem.GetByOrderIdAsync(orderId);
            return _mapper.Map<List<OrderDetailDto>>(items);
        }
        public async Task<List<OrderDto>> GetOrdersAsync(string userId)
        {
            var orders = await _uow.Order.GetOrdersAsync(userId);
            return _mapper.Map<List<OrderDto>>(orders);
        }
        public async Task<List<AdminOrderDto>> GetAllOrdersAsync()
        {
            var orders = await _uow.Order.GetAllAsync();

            return _mapper.Map<List<AdminOrderDto>>(orders);
        }
        public async Task<string> UpdateStatusAsync(int orderId, string status)
        {
            var order = await _uow.Order.GetByIdAsync(orderId);

            if (order == null)
                throw new Exception("Order not found.");

            var validStatuses = new[]
            {
                "Pending",
                "Processing",
                "Completed",
                "Cancelled"
            };

            if (!validStatuses.Contains(status))
                throw new Exception("Invalid status.");

            order.Status = status;

            await _uow.Order.UpdateAsync(order);

            await _uow.SaveAsync();

            return "Order status updated successfully.";
        }
    }
}
