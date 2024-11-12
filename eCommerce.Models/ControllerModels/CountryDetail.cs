using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerce.Models.ControllerModels
{
    [NotMapped]
    public class CountryDetail
    {
        public int CountryLinkId { get; set; }
        public string Country { get; set; }
        public string FlagCode { get; set; }
        public List<StateDetail> States { get; set; }
    }
}
