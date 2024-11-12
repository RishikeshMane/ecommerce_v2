using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eCommerce.Models.Models
{
    public class Category
    {
        public Category() { }
        public Category(string name)
        {
            Name = name; 
        }

        [Key]
        public int CategoryId { get; set; }
        public int CategoryLinkId { get; set; }

        [Required]
        public string Name { get; set; }

        public DateTime CreatedDateTime { get; set; } = DateTime.Now;
    }
}
