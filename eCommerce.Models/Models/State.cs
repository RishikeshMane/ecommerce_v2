using System.ComponentModel.DataAnnotations;

namespace eCommerce.Models.Models
{
    public class State
    {
        [Key]
        public int StateId { get; set; }
        public int StateLinkId { get; set; }
        public int CountryLinkId { get; set; }
        public string Name { get; set; }
    }
}
