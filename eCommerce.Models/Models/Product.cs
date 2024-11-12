///using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eCommerce.Models.Models
{
    public class Product
    {
        [Key]
        public long ProductId { get; set; }
        [Display(Name = "Category")]
        public int CategoryLinkId { get; set; }
        ///[ForeignKey("CategoryId")]
        ///[ValidateNever]
        ///public Category Category { get; set; }
        [Display(Name = "SubCategory")]
        public int SubCategoryLinkId { get; set; }
        ///[ForeignKey("SubCategoryId")]
        ///[ValidateNever]
        ///public SubCategory SubCategory { get; set; }
        public string? UserProductId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedDateTime { get; set; } = DateTime.Now;
    }
}
