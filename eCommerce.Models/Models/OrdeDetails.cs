using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eCommerce.Models.Models
{
    public class OrderDetails
    {
        [Key]
        public long OrderDetailsId { get; set; }
        public string UserOrderId { get; set; }
        public string? UserProductId { get; set; }
        public string Title { get; set; }
        public long SupplierMobileNo { get; set; }
        public int SizeLinkId { get; set; }
        public int ColorLinkId { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
        public int OrderStatusLinkId { get; set; } 
        public DateTime DeliveryDate { get; set; }
    }
}
