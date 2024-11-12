using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eCommerce.Models.ViewModels
{
    public class CategoryInfoList
    {
        public IEnumerable<CategoryInfo> categories { get; set; }
    }
}
