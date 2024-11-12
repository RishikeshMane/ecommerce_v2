using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerce.Models.ControllerModels
{
    [NotMapped]
    public class ProductDetails
    {
        public string UserProductId { get; set; }
        public long SupplierMobileno { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<ProductVariable> ProductVariables { get; set; } = new List<ProductVariable>();
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public int CategoryLinkId { get; set; }
        public int SubCategoryLinkId { get; set; }
        public List<ProductComments> Comments { get; set; } = new List<ProductComments>();
    }
}
