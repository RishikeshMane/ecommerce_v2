using System.ComponentModel.DataAnnotations;

namespace eCommerce.Models.Models
{
    public class SMSService
    {
        [Key]
        public int SMSServiceId { get; set; }
        public string SMSServiceLinkId { get; set; }
        public string SMSServiceName { get; set; }
        public int ActiveNow { get; set; }
    }
}
