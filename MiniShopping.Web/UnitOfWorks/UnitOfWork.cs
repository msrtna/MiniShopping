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

        public UnitOfWork(
            AppDbContext context,
            ICategoryRepository category,
            IProductRepository product,
            IBasketRepository basket)
        {
            _context = context;
            Category = category;
            Product = product;
            Basket = basket;
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
