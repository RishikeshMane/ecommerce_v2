using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerce.Models.ControllerModels
{
    [NotMapped]
    public class SupplierInfo
    {
        public string MobileNo { get; set; }
        public string City { get; set; }
        public string StoreName { get; set; }
    }
}
