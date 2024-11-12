using FileSystem.Models;

namespace FileSystem.Repository.IRepository
{
    public interface ISubCategoryRepository : IRepository<SubCategory>
    {
        public void Update(SubCategory obj);
        public void Save();
    }
}
