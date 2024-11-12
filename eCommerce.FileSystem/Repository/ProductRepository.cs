using FileSystem.Data;
using FileSystem.Models;
using FileSystem.Repository.IRepository;

namespace FileSystem.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        FileSystemDbContext _db;
        public ProductRepository(FileSystemDbContext db) : base(db)
        {
            _db = db;
        }
        public void Save()
        {
            _db.SaveChanges();
        }

        public void Update(Product obj)
        {
            _db.Product.Update(obj);
        }
    }
}
