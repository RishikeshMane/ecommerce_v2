using System.ComponentModel.DataAnnotations;

namespace eCommerce.Models.Models
{
    public class Country
    {
        [Key]
        public int CountryId { get; set; }
        public int CountryLinkId { get; set; }
        public string Name { get; set; }
        public string FlagCode { get; set; }
    }
}
