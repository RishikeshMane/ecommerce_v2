using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerce.Models.ControllerModels
{
    [NotMapped]
    public class VariableIndex
    {
        public string ProductId { get; set; }
        public int Index { get; set; }
        public List<string> ImagesUrls { get; set; } = new List<string>();
    }
}
