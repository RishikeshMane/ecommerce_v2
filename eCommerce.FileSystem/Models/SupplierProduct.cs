using System.ComponentModel.DataAnnotations;

namespace FileSystem.Models
{
    public class SupplierProduct
    {
        [Key]
        public long SupplierProductId { get; set; }
        public long MobileNo { get; set; }
        public string UserProductId { get; set; }

    }
}
