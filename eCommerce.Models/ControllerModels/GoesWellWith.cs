using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerce.Models.ControllerModels
{
    [NotMapped]
    public class GoesWellWith
    {
        public string ProductId { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public int Pincode { get; set; }
        public string Store { get; set; }
        public string City { get; set; }
        public string Image { get; set; }
        public int Price { get; set; }
    }
}
