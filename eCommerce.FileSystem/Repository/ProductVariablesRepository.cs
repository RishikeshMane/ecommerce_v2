using FileSystem.Data;
using FileSystem.Models;
using FileSystem.Repository.IRepository;

namespace FileSystem.Repository
{
    public class ProductVariablesRepository : Repository<ProductVariables>, IProductVariablesRepository
    {
        FileSystemDbContext _db;
        public ProductVariablesRepository(FileSystemDbContext db) : base(db)
        {
            _db = db;
        }
        public void Save()
        {
            _db.SaveChanges();
        }

        public void Update(ProductVariables obj)
        {
            _db.ProductVariables.Update(obj);
        }
    }
}
