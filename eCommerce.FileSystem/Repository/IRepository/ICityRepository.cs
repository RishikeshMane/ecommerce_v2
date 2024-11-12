using FileSystem.Models;

namespace FileSystem.Repository.IRepository
{
    public interface ICityRepository : IRepository<City>
    {
        public void Update(City obj);
        public void Save();
    }
}
