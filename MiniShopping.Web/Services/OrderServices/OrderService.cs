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

        public OrderService(IUnitOfWork uow, AppDbContext context)
        {
            _uow = uow;
            _context = context;
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

                decimal totalAmount = 0;

                // بررسی موجودی محصولات
                foreach (var item in basketItems)
                {
                    var product =
                        await _uow.Product.GetByIdAsync(item.ProductId);

                    if (product == null)
                        throw new Exception(
                            $"Product {item.ProductId} not found.");

                    if (product.Quantity < item.Quantity)
                        throw new Exception(
                            $"Not enough stock for {product.Name}.");

                    totalAmount +=
                        item.Quantity * product.Price;
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
                    var product =
                        await _uow.Product.GetByIdAsync(item.ProductId);

                    orderItems.Add(new OrderItem
                    {
                        OrderId = order.Id,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        UnitPrice = product!.Price
                    });

                    // کاهش موجودی
                    product.Quantity -= item.Quantity;

                    await _uow.Product.UpdateAsync(product);

                    // حذف از سبد
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
            return items.Select(o => new OrderDetailDto
            {
                ProductName = o.Product.Name,
                Quantity = o.Quantity,
                UnitPrice = o.UnitPrice
            }).ToList();
        }
        public async Task<List<OrderDto>> GetOrdersAsync(string userId)
        {
            var orders = await _uow.Order.GetOrdersAsync(userId);
            return orders.Select(o => new OrderDto
            {
                Id = o.Id,
                Status = o.Status,
                OrderDate = o.OrderDate,
                TotalAmount = o.TotalAmount
            }).ToList();
        }
    }
}
