using System.ComponentModel.DataAnnotations;

namespace eCommerce.Models.Models
{
    public class UserRole
    {
        [Key]
        public int UserRoleId { get; set; }

        public string Role { get; set; }
    }
}
