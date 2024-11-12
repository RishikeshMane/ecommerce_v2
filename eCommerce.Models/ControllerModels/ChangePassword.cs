using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerce.Models.ControllerModels
{
    [NotMapped]
    public class ChangePassword
    {
        public string MobileNo { get; set; }
        public string ExistingPassword { get; set; }
        public string NewPassword { get; set; }
        public string OTP { get; set; }
    }
}
