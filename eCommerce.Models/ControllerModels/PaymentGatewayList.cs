using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerce.Models.ControllerModels
{
    [NotMapped]
    public class PaymentGatewayList
    {
        public List<PaymentGatewayDetail> PaymentGatewayDetail { get; set; }
    }

    [NotMapped]
    public class PaymentGatewayDetail
    {
        public int PaymentGatewayLinkId { get; set; }
        public string PaymentGatewayName { get; set; }
        public int ActiveNow { get; set; }
    }


}
