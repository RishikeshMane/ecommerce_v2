using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eCommerce.Models.Models
{
    public class ProductVariables
    {
        [Key]
        public long ProductVariableId { get; set; }
        public string UserProductId { get; set; }
        public int SizeLinkId { get; set; }
        public int ColorLinkId { get; set; }
        public int Price { get; set; }
        public int Discount { get; set; }
        public string ImageUrls { get; set; }
        public int Inventory { get; set; }
    }
}
