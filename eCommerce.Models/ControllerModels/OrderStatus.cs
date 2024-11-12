using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerce.Models.ControllerModels
{
    [NotMapped]
    public class OrderStatusList
    {
        public List<OrderStatusDetail> OrderStatusDetail { get; set; }
    }

    [NotMapped]
    public class OrderStatusDetail
    {
        public int OrderStatusLinkId { get; set; }
        public string OrderStatus { get; set; }
    }
}
