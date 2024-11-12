using System.ComponentModel.DataAnnotations;

namespace FileSystem.Models
{
    public class Color
    {
        [Key]
        public int ColorId { get; set; }
        public int ColorLinkId { get; set; }
        public int Red { get; set; }
        public int Green { get; set; }
        public int Blue { get; set; }
        public string Description { get; set; }
    }
}
