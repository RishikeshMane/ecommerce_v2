using System.ComponentModel.DataAnnotations;

namespace FileSystem.Models
{
    public class User
    {
        [Key]
        public long UserId { get; set; }
        public int UserRoleId { get; set; }
        public string FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? StoreName { get; set; }
        public string Address1 { get; set; }
        public string? Address2 { get; set; }
        public int CityId { get; set; }
        public int StateId { get; set; }
        public int CountryId { get; set; }

        [Range(100000, 999999)]
        public int PinCode { get; set; }

        [Range(00000000, 9999999999)]
        public long Phone1 { get; set; }

        [Range(10000000, 99999999)]
        public int? Phone2 { get; set; }
        public string DeliveryPincodes { get; set; } // Migration Fifth
        public string FlagCode { get; set; }

        public DateTime CreatedDateTime { get; set; } = DateTime.Now;
    }
}
