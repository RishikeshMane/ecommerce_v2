using System.ComponentModel.DataAnnotations;

namespace eCommerce.Models.Models
{
    public class EMailJS
    {
        [Key]
        public int EMailJSId { get; set; }
        public string KeyId { get; set; }
        public string ServiceId { get; set; }
        public string TemplateId { get; set; }
    }
}
