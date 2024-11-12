using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eCommerce.Models.Models
{
    public class SupplierProduct
    {
        [Key]
        public long SupplierProductId { get; set; }
        public long MobileNo { get; set; }
        public string UserProductId { get; set; }

    }
}
