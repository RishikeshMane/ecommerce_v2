using FileSystem.Data;
using FileSystem.Repository.IRepository;

namespace FileSystem.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private FileSystemDbContext _db;
        public IProductRepository ProductRepository { get; set; }
        public IProductVariablesRepository ProductVariablesRepository { get; set; }
        public ISupplierProductRepository SupplierProductRepository { get; set; }
        public ICategoryRepository CategoryRepository { get; set; }
        public ISubCategoryRepository SubCategoryRepository { get; set; }
        public ICityRepository CityRepository { get; set; }
        public IUserRepository UserRepository { get; set; }
        public ISizeRepository SizeRepository { get; set; }
        public IColorRepository ColorRepository { get; set; }
        public IShoppingCartRepository ShoppingCartRepository { get; set; }
        public UnitOfWork(FileSystemDbContext db)
        {
            _db = db;

            ProductRepository = new ProductRepository(db);
            ProductVariablesRepository = new ProductVariablesRepository(db);
            SupplierProductRepository = new SupplierProductRepository(db);
            CategoryRepository = new CategoryRepository(db);
            SubCategoryRepository = new SubCategoryRepository(db);
            CityRepository = new CityRepository(db);
            UserRepository = new UserRepository(db);
            SizeRepository = new SizeRepository(db);
            ColorRepository = new ColorRepository(db);
            ShoppingCartRepository = new ShoppingCartRepository(db);
        }

        void IUnitOfWork.Save()
        {
            _db.SaveChanges();
        }

    }
}
