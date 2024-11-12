using FileSystem.Models;

namespace FileSystem.Repository.IRepository
{
    public interface IProductRepository : IRepository<Product>
    {
        public void Update(Product obj);
        public void Save();
    }
}
