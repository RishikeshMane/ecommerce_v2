using FileSystem.Data;
using FileSystem.Models;
using FileSystem.Repository.IRepository;

namespace FileSystem.Repository
{
    public class SizeRepository : Repository<Size>, ISizeRepository
    {
        FileSystemDbContext _db;
        public SizeRepository(FileSystemDbContext db) : base(db)
        {
            _db = db;
        }
        public void Save()
        {
            _db.SaveChanges();
        }

        public void Update(Size obj)
        {
            _db.Size.Update(obj);
        }
    }
}
