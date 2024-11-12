using FileSystem.Models;

namespace FileSystem.Repository.IRepository
{
    public interface IProductVariablesRepository : IRepository<ProductVariables>
    {
        public void Update(ProductVariables obj);
        public void Save();
    }
}
