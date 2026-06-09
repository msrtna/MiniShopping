using MiniShopping.Web.Models;

namespace MiniShopping.Web.Repositories
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetAllAsync();
        Task<Category?> GetByIdAsync(int id);
        Task AddAsync(Category category);
        Task UpdateAsinc(Category category);
        Task DeleteAsync(int id);
    }
}
