using System.ComponentModel.DataAnnotations;

namespace eCommerce.Models.Models
{
    public class Password
    {
        [Key]
        public long PasswordId { get; set; }

        [Display(Name = "User")]
        public long UserId { get; set; }
        ///[ForeignKey("UserId")]
        ///[ValidateNever]
        ///public User User { get; set; }

        public string PasswordKey { get; set; }
        public string OTP { get; set; }
    }
}
