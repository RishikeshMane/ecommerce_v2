using FileSystem.Data;
using FileSystem.Models;
using FileSystem.Repository.IRepository;

namespace FileSystem.Repository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        FileSystemDbContext _db;
        public UserRepository(FileSystemDbContext db) : base(db)
        {
            _db = db;
        }
        public void Save()
        {
            _db.SaveChanges();
        }

        public void Update(User obj)
        {
            _db.User.Update(obj);
        }
    }
}
