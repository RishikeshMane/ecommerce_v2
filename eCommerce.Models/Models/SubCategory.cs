///using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eCommerce.Models.Models
{
    public class SubCategory
    {
        public SubCategory() {}

        [Key]
        public int SubCategoryId { get; set; }

        [Display(Name = "Category")]
        public int CategoryLinkId { get; set; }
        ///[ForeignKey("CategoryId")]
        ///[ValidateNever]
        ///public Category Category { get; set; }
        public int SubCategoryLinkId { get; set; }

        [Required]
        public string Name { get; set; }

        public DateTime CreatedDateTime { get; set; } = DateTime.Now;
    }
}
