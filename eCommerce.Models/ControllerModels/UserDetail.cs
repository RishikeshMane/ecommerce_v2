using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using eCommerce.Models.ControllerModels;

namespace eCommerce.Models.Models
{
    [NotMapped]
    public class UserDetail
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public int Pincode { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Store { get; set; }
        public string UserRole { get; set; }
        public bool Subscribe { get; set; }
        public List<string> DeliveryPinCodes { get; set; } = new List<string>();
        public int MoreAddressCount { get; set; }
        public string FlagCode { get; set; }
        public AddressList Address { get; set; } = new AddressList();
    }
}
