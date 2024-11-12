using System.ComponentModel.DataAnnotations.Schema;

namespace FileSystem.ControllerModels
{
    [NotMapped]
    public class ProductList
    {
        public List<Products> Products { get; set; }
    }
}
