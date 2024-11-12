namespace FileSystem.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IProductRepository ProductRepository { get; set; }
        IProductVariablesRepository ProductVariablesRepository { get; set; }
        ISupplierProductRepository SupplierProductRepository { get; set; }
        ICategoryRepository CategoryRepository { get; set; }
        ISubCategoryRepository SubCategoryRepository { get; set; }
        ICityRepository CityRepository { get; set; }
        IUserRepository UserRepository { get; set; }
        ISizeRepository SizeRepository { get; set; }
        IColorRepository ColorRepository { get; set; }
        IShoppingCartRepository ShoppingCartRepository { get; set; }
        public void Save();
    }

}
