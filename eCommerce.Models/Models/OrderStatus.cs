using System.ComponentModel.DataAnnotations;

namespace eCommerce.Models.Models
{
    public class OrderStatus
    {
        [Key]
        public int OrderStatusId { get; set; }
        public int OrderStatusLinkId { get; set; }
        public string Description { get; set; }

    }
}
