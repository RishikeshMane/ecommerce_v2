using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerce.Models.ControllerModels
{
    [NotMapped]
    public class CityDetail
    {
        public int CityLinkId { get; set; }
        public string City { get; set; }
    }
}
