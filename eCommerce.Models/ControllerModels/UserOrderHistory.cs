using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerce.Models.ControllerModels
{
    [NotMapped]
    public class UserOrderHistory
    {
        public string UserOrderId { get; set; }
        public string Address { get; set; }
        public string Price { get; set; }
        public string OrderDate { get; set; }
        public bool CanCancelOrder { get; set; }
        public string DeliveryDate { get; set; }
        public string DeliveryStatus { get; set; }
    }
}
