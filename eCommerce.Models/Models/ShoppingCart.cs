using System.ComponentModel.DataAnnotations;

namespace eCommerce.Models.Models
{
    public class ShoppingCart
    {
        [Key]
        public long ShoppingCartId { get; set; }

        [Range(00000000, 9999999999)]
        public long MobileNo { get; set; }
        [Range(00000000, 9999999999)]
        public long SupplierMobileNo { get; set; }
        public string? UserProductId { get; set; }
        public int SizeLinkId { get; set; }
        public int ColorLinkId { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
    }
}
