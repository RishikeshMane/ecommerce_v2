using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerce.Models.ControllerModels
{
    [NotMapped]
    public class ShoppingAddress
    {
        public int SelectedCountryLinkId { get; set; }
        public string SelectedCountry { get; set; }
        public int SelectedStateLinkId { get; set; }
        public string SelectedState { get; set; }
        public int SelectedCityLinkId { get; set; }
        public string SelectedCity { get; set; }
        public int Pincode { get; set; }
        public string Address { get; set; }
        public string FlagCode { get; set; }
    }
}
