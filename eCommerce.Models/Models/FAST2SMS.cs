using System.ComponentModel.DataAnnotations;

namespace eCommerce.Models.Models
{
    public class FAST2SMS
    {
        [Key]
        public int FAST2SMSId { get; set; }
        public string KeyId { get; set; }
    }
}
