using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerce.Models.ControllerModels
{
    [NotMapped]
    public class PaymentRangeList
    {
        public List<PaymentRangeDetail> PaymentRangeDetail { get; set; }
    }

    [NotMapped]
    public class PaymentRangeDetail
    {
        public int PaymentRangeLinkId { get; set; }
        public int MinimumValue { get; set; }
        public int MaximumValue { get; set; }
        public int PercentageFees { get; set; }
    }
}
