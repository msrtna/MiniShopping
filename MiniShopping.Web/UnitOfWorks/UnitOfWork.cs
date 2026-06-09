using MiniShopping.Web.Data;
using MiniShopping.Web.Repositories;

namespace MiniShopping.Web.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public ICategoryRepository Category { get; }

        public UnitOfWork(AppDbContext context, ICategoryRepository category)
        {
            _context = context;
            Category = category;
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
