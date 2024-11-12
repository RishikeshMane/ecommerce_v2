using System.ComponentModel.DataAnnotations;

namespace eCommerce.Models.Models
{
    public class PaymentGateway
    {
        [Key]
        public int PaymentGatewayId { get; set; }
        public string PaymentGatewayLinkId { get; set; }
        public string PaymentGatewayName { get; set; }
        public int ActiveNow { get; set; }
    }
}
