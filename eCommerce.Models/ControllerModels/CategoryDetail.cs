using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerce.Models.ControllerModels
{
    [NotMapped]
    public class CategoryDetail
    {
        public string Category { get; set; }
        public int CategoryLinkId { get; set; }
        public List<int> SubCategoryLinkIds { get; set; } = new List<int>();
        public List<string> SubCategories { get; set; } = new List<string>();
    }
}
