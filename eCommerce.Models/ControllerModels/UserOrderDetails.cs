using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerce.Models.ControllerModels
{
    [NotMapped]
    public class UserOrderDetails
    {
        public string UserOrderId { get; set; }
        public string Title { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public string Quantity { get; set; }
        public string Price { get; set; }
    }
}
