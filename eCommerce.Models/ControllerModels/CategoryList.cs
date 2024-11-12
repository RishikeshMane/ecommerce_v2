using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerce.Models.ControllerModels
{
    [NotMapped]
    public class CategoryList
    {
        public List<CategoryDetail> Category { get; set; } = new List<CategoryDetail>();
    }
}
