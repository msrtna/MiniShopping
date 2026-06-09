using MiniShopping.Web.Repositories;

namespace MiniShopping.Web.UnitOfWorks
{
    public interface IUnitOfWork
    {
        public ICategoryRepository Category { get; }
        public IProductRepository Product { get; }

        public Task<int> SaveAsync();
    }
}
