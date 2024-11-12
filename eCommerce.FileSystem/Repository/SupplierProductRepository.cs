using FileSystem.Data;
using FileSystem.Models;
using FileSystem.Repository.IRepository;

namespace FileSystem.Repository
{
    public class SupplierProductRepository : Repository<SupplierProduct>, ISupplierProductRepository
    {
        FileSystemDbContext _db;
        public SupplierProductRepository(FileSystemDbContext db) : base(db)
        {
            _db = db;
        }
        public void Save()
        {
            _db.SaveChanges();
        }

        public void Update(SupplierProduct obj)
        {
            _db.SupplierProduct.Update(obj);
        }
    }
}
