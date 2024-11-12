using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerce.Models.ControllerModels
{
    [NotMapped]
    public class UserOrderDetail
    {
        public string UserOrderId { get; set; }
        public string UserProductId { get; set; }
        public string Title { get; set; }
        public string Address { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public string Price { get; set; }
        public string Count { get; set; }
        public string OrderDate { get; set; }
        public string DeliveryDate { get; set; }
        public string DeliveryStatus { get; set; }
    }
}
