using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eCommerce.Models.Models
{
    public class OrderHeader
    {
        [Key]
        public long OrderHeaderId { get; set; }

        [Range(00000000, 9999999999)]
        public long MobileNo { get; set; }
        public string UserOrderId { get; set; }
        public string OrderId { get; set; }
        public string PaymentId { get; set; }
        public string ReceiptId { get; set; }
        public string TransactionId { get; set; }
        public string GatewayLinkId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailId { get; set; }
        public int CountryId { get; set; }
        public int StateId { get; set; }
        public int CityId { get; set; }
        public string Address { get; set; }
        public int Pincode { get; set; }
        public DateTime OrderDate { get; set; }
        public string Comments { get; set; }
    }
}
