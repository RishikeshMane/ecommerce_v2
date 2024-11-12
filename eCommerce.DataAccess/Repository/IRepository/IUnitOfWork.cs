using eCommerce.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        ICategoryRepository CategoryRepository { get; set; }
        ISubCategoryRepository SubCategoryRepository { get; set; }
        ICountryRepository CountryRepository { get; set; }
        IStateRepository StateRepository { get; set; }
        ICityRepository CityRepository { get; set; }
        IUserRoleRepository UserRoleRepository { get; set; }
        IUserRepository UserRepository { get; set; }
        IPasswordRepository PasswordRepository { get; set; }
        ISizeRepository SizeRepository { get; set; }
        IColorRepository ColorRepository { get; set; }
        IProductRepository ProductRepository { get; set; }
        IProductVariablesRepository ProductVariablesRepository { get; set; }
        IProductCommentRepository ProductCommentRepository { get; set; }
        ISupplierProductRepository SupplierProductRepository { get; set; }
        IShoppingCartRepository ShoppingCartRepository { get; set; }
        IAddressRepository AddressRepository { get; set; }
        IPaymentGatewayRepository PaymentGatewayRepository { get; set; }
        IRazorpayRepository RazorpayRepository { get; set; }
        IEMailServiceRepository EMailServiceRepository { get; set; }
        IEMailJSRepository EMailJSRepository { get; set; }
        ISMSServiceRepository SMSServiceRepository { get; set; }
        IFAST2SMSRepository FAST2SMSRepository { get; set; }
        IOrderStatusRepository OrderStatusRepository { get; set; }
        IOrderHeaderRepository OrderHeaderRepository { get; set; }
        IOrderDetailsRepository OrderDetailsRepository { get; set; }
        IPaymentStatusRepository PaymentStatusRepository { get; set; }
        IPaymentRangeRepository PaymentRangeRepository { get; set; }
        IPaymentsRepository PaymentsRepository { get; set; }

        public void Save();
    }
}
