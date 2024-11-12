using BulkyBook.DataAccess.Repository;
using eCommerce.Data;
using eCommerce.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private eCommerceDbContext _db;
        public ICategoryRepository CategoryRepository { get; set; }
        public ISubCategoryRepository SubCategoryRepository { get; set; }
        public ICountryRepository CountryRepository { get; set; }
        public IStateRepository StateRepository { get; set; }
        public ICityRepository CityRepository { get; set; }
        public IUserRoleRepository UserRoleRepository { get; set; }
        public IUserRepository UserRepository { get; set; }
        public IPasswordRepository PasswordRepository { get; set; }
        public ISizeRepository SizeRepository { get; set; }
        public IColorRepository ColorRepository { get; set; }
        public IProductRepository ProductRepository { get; set; }
        public IProductVariablesRepository ProductVariablesRepository { get; set; }
        public IProductCommentRepository ProductCommentRepository { get; set; }
        public ISupplierProductRepository SupplierProductRepository { get; set; }
        public IShoppingCartRepository ShoppingCartRepository { get; set; }
        public IAddressRepository AddressRepository { get; set; }
        public IPaymentGatewayRepository PaymentGatewayRepository { get; set; }
        public IRazorpayRepository RazorpayRepository { get; set; }
        public IEMailServiceRepository EMailServiceRepository { get; set; }
        public IEMailJSRepository EMailJSRepository { get; set; }
        public ISMSServiceRepository SMSServiceRepository { get; set; }
        public IFAST2SMSRepository FAST2SMSRepository { get; set; }
        public IOrderStatusRepository OrderStatusRepository { get; set; }
        public IOrderHeaderRepository OrderHeaderRepository { get; set; }
        public IOrderDetailsRepository OrderDetailsRepository { get; set; }
        public IPaymentStatusRepository PaymentStatusRepository { get; set; }
        public IPaymentRangeRepository PaymentRangeRepository { get; set; }
        public IPaymentsRepository PaymentsRepository { get; set; }

        public UnitOfWork(eCommerceDbContext db)
        {
            _db = db;

            CategoryRepository = new CategoryRepository(db);
            SubCategoryRepository = new SubCategoryRepository(db);
            CountryRepository = new CountryRepository(db);
            StateRepository = new StateRepository(db);
            CityRepository = new CityRepository(db);
            UserRoleRepository = new UserRoleRepository(db);
            UserRepository = new UserRepository(db);
            PasswordRepository = new PasswordRepository(db);
            SizeRepository = new SizeRepository(db);
            ColorRepository = new ColorRepository(db);
            ProductRepository = new ProductRepository(db);
            ProductVariablesRepository = new ProductVariableRepository(db);
            ProductCommentRepository = new ProductCommentRepository(db);
            SupplierProductRepository = new SupplierProductRepository(db);
            ShoppingCartRepository = new ShoppingCartRepository(db);
            AddressRepository = new AddressRepository(db);
            PaymentGatewayRepository = new PaymentGatewayRepository(db);
            RazorpayRepository = new RazorpayRepository(db);
            EMailServiceRepository = new EMailServiceRepository(db);
            EMailJSRepository = new EMailJSRepository(db);
            SMSServiceRepository = new SMSServiceRepository(db);
            FAST2SMSRepository = new FAST2SMSRepository(db);
            OrderStatusRepository = new OrderStatusRepository(db);
            OrderHeaderRepository = new OrderHeaderRepository(db);
            OrderDetailsRepository = new OrderDetailsRepository(db);
            PaymentStatusRepository = new PaymentStatusRepository(db);
            PaymentRangeRepository = new PaymentRangeRepository(db);
            PaymentsRepository = new PaymentsRepository(db);
        }

        void IUnitOfWork.Save()
        {
            _db.SaveChanges();
        }
    }
}
