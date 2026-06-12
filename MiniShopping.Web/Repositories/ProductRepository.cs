using Microsoft.EntityFrameworkCore;
using MiniShopping.Web.Data;
using MiniShopping.Web.Models;

namespace MiniShopping.Web.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;
        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await _context.Products.Include(p=> p.Category).ToListAsync();
        }
        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Products.Include(p=> p.Category).FirstOrDefaultAsync(x=> x.Id == id);
        }
        public async Task AddAsync(Product product)
        {
            await _context.Products.AddAsync(product);
        }
        public Task UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            return Task.CompletedTask;
        }
        public async Task DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
                _context.Products.Remove(product);
        }

        public async Task<IQueryable<Product>> GetQueryableAsync()
        {
            return _context.Products.Include(p => p.Category);
        }
    }
}
