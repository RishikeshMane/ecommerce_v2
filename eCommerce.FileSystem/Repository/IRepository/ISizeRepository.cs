using FileSystem.Models;

namespace FileSystem.Repository.IRepository
{
    public interface ISizeRepository : IRepository<Size>
    {
        public void Update(Size obj);
        public void Save();
    }
}
