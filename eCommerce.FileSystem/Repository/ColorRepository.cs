using FileSystem.Data;
using FileSystem.Models;
using FileSystem.Repository.IRepository;

namespace FileSystem.Repository
{
    public class ColorRepository : Repository<Color>, IColorRepository
    {
        FileSystemDbContext _db;
        public ColorRepository(FileSystemDbContext db) : base(db)
        {
            _db = db;
        }
        public void Save()
        {
            _db.SaveChanges();
        }

        public void Update(Color obj)
        {
            _db.Color.Update(obj);
        }
    }
}
