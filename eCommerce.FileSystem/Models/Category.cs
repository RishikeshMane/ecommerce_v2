using System.ComponentModel.DataAnnotations;

namespace FileSystem.Models
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
