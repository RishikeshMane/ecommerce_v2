using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eCommerce.Models.ViewModels
{
    public class CategoryInfo
    {
        public string CategoryName { get; set; }
        public string SubCategoryName { get; set; }

        public DateTime CreatedDateTime { get; set; } = DateTime.Now;
    }
}
