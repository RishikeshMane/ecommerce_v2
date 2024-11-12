using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerce.Models.ControllerModels
{
    [NotMapped]
    public class SizeDetail
    {
        public int sizeId { get; set; }
        public int SizeLinkId { get; set; }
        public string Sizecode { get; set; }
        public string Description { get; set; }
    }
}
