using FileSystem.Data;
using FileSystem.Models;
using FileSystem.Repository.IRepository;

namespace FileSystem.Repository
{
    public class SubCategoryRepository : Repository<SubCategory>, ISubCategoryRepository
    {
        FileSystemDbContext _db;
        public SubCategoryRepository(FileSystemDbContext db) : base(db)
        {
            _db = db;
        }
        public void Save()
        {
            _db.SaveChanges();
        }

        public void Update(SubCategory obj)
        {
            _db.SubCategory.Update(obj);
        }
    }
}
