using FileSystem.Data;
using FileSystem.Models;
using FileSystem.Repository.IRepository;

namespace FileSystem.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        FileSystemDbContext _db;
        public CategoryRepository(FileSystemDbContext db) : base(db)
        {
            _db = db;
        }
        public void Save()
        {
            _db.SaveChanges();
        }

        public void Update(Category obj)
        {
            _db.Category.Update(obj);
        }
    }
}
