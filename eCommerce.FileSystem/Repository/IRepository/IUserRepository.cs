using FileSystem.Models;

namespace FileSystem.Repository.IRepository
{
    public interface IUserRepository : IRepository<User>
    {
        public void Update(User obj);
        public void Save();
    }
}
