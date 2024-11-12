using FileSystem.Models;

namespace FileSystem.Repository.IRepository
{
    public interface IColorRepository : IRepository<Color>
    {
        public void Update(Color obj);
        public void Save();
    }
}
