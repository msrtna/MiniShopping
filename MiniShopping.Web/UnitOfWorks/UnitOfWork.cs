using MiniShopping.Web.Data;
using MiniShopping.Web.Repositories;

namespace MiniShopping.Web.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public ICategoryRepository Category { get; }
        public IProductRepository Product { get; }
        public IBasketRepository Basket { get; }
        public IOrderRepository Order { get; }
        public IOrderItemRepository OrderItem { get; }

        public UnitOfWork(
            AppDbContext context,
            ICategoryRepository category,
            IProductRepository product,
            IBasketRepository basket,
            IOrderRepository order,
            IOrderItemRepository orderItem)
        {
            _context = context;
            Category = category;
            Product = product;
            Basket = basket;
            Order = order;
            OrderItem = orderItem;
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
