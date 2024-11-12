using System.ComponentModel.DataAnnotations.Schema;

namespace FileSystem.ControllerModels
{
    [NotMapped]
    public class ProductComments
    {
        public string UserProductId { get; set; }
        public string UserId { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Comment { get; set; }
        public int Rating { get; set; }
        public string Date { get; set; }
    }
}
