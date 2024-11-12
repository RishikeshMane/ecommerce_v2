using FileSystem.Models;

namespace FileSystem.Repository.IRepository
{
    public interface ISupplierProductRepository : IRepository<SupplierProduct>
    {
        public void Update(SupplierProduct obj);
        public void Save();
    }
}
