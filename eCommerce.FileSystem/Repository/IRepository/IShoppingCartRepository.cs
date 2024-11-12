using FileSystem.Models;

namespace FileSystem.Repository.IRepository
{
    public interface IShoppingCartRepository : IRepository<ShoppingCart>
    {
        public void Update(ShoppingCart obj);
        public void Save();
    }
}
