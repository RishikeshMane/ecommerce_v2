using System.ComponentModel.DataAnnotations;

namespace eCommerce.Models.Models
{
    public class Razorpay
    {
        [Key]
        public int RazorpayId { get; set; }
        public string KeyId { get; set; }
        public string KeySecret { get; set; }
        public int ActiveNow { get; set; }
    }
}
