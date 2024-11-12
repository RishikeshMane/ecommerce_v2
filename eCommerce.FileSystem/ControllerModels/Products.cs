using System.ComponentModel.DataAnnotations.Schema;

namespace FileSystem.ControllerModels
{
    [NotMapped]
    public class Products
    {
        public string ProductId { get; set; }
        public string Title { get; set; }
        public string City { get; set; }
        public int Pincode { get; set; }
        public string Store { get; set; }
        public string Image { get; set; }
        public int Price { get; set; }
        public int Count { get; set; }
    }
}
