using System.ComponentModel.DataAnnotations.Schema;

namespace FileSystem.ControllerModels
{
    [NotMapped]
    public class ProductDetail
    {
        public long UserMobileNo { get; set; }
        public string addUpdate { get; set; }
        public string UserProductId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<ProductVariable> ProductVariables { get; set; } = new List<ProductVariable>();
        public int CategoryLinkId { get; set; }
        public int SubCategoryLinkId { get; set; }
        ///public List<IFormFile> Files { get; set; } = new List<IFormFile>();
        ///public IFormFile Files { get; set; }
    }
}
