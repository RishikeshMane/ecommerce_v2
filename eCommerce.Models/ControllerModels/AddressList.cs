using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerce.Models.ControllerModels
{
    [NotMapped]
    public class AddressList
    {
        public List<AddressDetail> Addresses { get; set; } = new List<AddressDetail>();
    }

    [NotMapped]
    public class AddressDetail
    {
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Pincode { get; set; }
        public string Address { get; set; }
        public int IsDeliveryAddress { get; set; }
        public string FlagCode { get; set; }
    }
}
