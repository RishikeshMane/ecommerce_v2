using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerce.Models.ControllerModels
{
    [NotMapped]
    public class StateDetail
    {
        public int StateLinkId { get; set; }
        public string State { get; set; }
        public List<CityDetail> Cities { get; set; }
    }
}
