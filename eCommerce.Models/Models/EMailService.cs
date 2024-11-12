using System.ComponentModel.DataAnnotations;

namespace eCommerce.Models.Models
{
    public class EMailService
    {
        [Key]
        public int EMailServiceId { get; set; }
        public string EMailServiceLinkId { get; set; }
        public string EMailServiceName { get; set; }
        public int ActiveNow { get; set; }
    }
}
