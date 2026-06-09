using MiniShopping.Web.Data;
using MiniShopping.Web.Repositories;

namespace MiniShopping.Web.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public ICategoryRepository Category { get; }

        public IProductRepository Product { get; }

        public UnitOfWork(
            AppDbContext context,
            ICategoryRepository category,
            IProductRepository product)
        {
            _context = context;
            Category = category;
            Product = product;
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
