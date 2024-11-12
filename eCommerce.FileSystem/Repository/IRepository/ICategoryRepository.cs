using FileSystem.Models;

namespace FileSystem.Repository.IRepository
{
    public interface ICategoryRepository : IRepository<Category>
    {
        public void Update(Category obj);
        public void Save();
    }
}
