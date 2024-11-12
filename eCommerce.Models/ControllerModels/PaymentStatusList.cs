using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerce.Models.ControllerModels
{
    [NotMapped]
    public class PaymentStatusList
    {
        public List<PaymentStatusDetail> PaymentStatusDetail { get; set; }
    }

    [NotMapped]
    public class PaymentStatusDetail
    {
        public int PaymentStatusLinkId { get; set; }
        public string PaymentStatus { get; set; }
    }
}
