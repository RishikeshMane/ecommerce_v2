using System.ComponentModel.DataAnnotations;

namespace eCommerce.Models.Models
{
    public class ProductComment
    {
        [Key]
        public long ProductCommentId { get; set; }

        [Display(Name = "Product")]
        public string UserProductId { get; set; }
        public long UserId { get; set; }
        public int Rating { get; set; }
        public int Verified { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedDateTime { get; set; } = DateTime.Now;
    }
}
