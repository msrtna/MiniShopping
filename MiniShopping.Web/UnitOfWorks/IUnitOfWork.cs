using MiniShopping.Web.Repositories;

namespace MiniShopping.Web.UnitOfWorks
{
    public interface IUnitOfWork
    {
        public ICategoryRepository Category { get; }
        public IProductRepository Product { get; }
        public IBasketRepository Basket { get; }
        public IOrderRepository Order { get; }
        public IOrderItemRepository OrderItem  { get; }

        public Task<int> SaveAsync();
    }
}
