using System.ComponentModel.DataAnnotations;

namespace eCommerce.Models.Models
{
    public class Address
    {
        [Key]
        public long AddressId { get; set; }
        public int IsDeliveryAddress { get; set; }
        [Range(00000000, 9999999999)]
        public long PhoneNo { get; set; }
        public string Addresss { get; set; }
        public int CityId { get; set; }
        public int StateId { get; set; }
        public int CountryId { get; set; }
        [Range(100000, 999999)]
        public int PinCode { get; set; }
        public string FlagCode { get; set; }
    }
}
