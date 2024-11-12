using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerce.Models.ControllerModels
{
    [NotMapped]
    public class Products
    {
        public string ProductId { get; set; }
        public string Title { get; set; }
        public string City { get; set; }
        public int Pincode { get; set; }
        public string Store { get; set; }
        public string Image { get; set; }
        public int Price { get; set; }
        public int Count { get; set; }
    }
}
