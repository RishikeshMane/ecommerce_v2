using FileSystem.Data;
using FileSystem.Models;
using FileSystem.Repository.IRepository;

namespace FileSystem.Repository
{
    public class CityRepository : Repository<City>, ICityRepository
    {
        FileSystemDbContext _db;
        public CityRepository(FileSystemDbContext db) : base(db)
        {
            _db = db;
        }
        public void Save()
        {
            _db.SaveChanges();
        }

        public void Update(City obj)
        {
            _db.City.Update(obj);
        }
    }
}
