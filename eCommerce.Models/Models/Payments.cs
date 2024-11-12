using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eCommerce.Models.Models
{
    public class Payments
    {
        public long PaymentsId { get; set; }
        public string UserOrderId { get; set; }
        public long SupplierMobileNo { get; set; }
        public int Price { get; set; }
        public string SupplierPayment { get; set; }
        public DateTime SupplierPaymentDate { get; set; }
        public int SupplierPaymenStatus { get; set; }
        public string CompanyPayment { get; set; }
        public DateTime CompanyPaymentDate { get; set; }
        public int CompanyPaymenStatus { get; set; }
        public string SupplierCompanyTransactionId { get; set; }
        public string Comments { get; set; }
    }
}
