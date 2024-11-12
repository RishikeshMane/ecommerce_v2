using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace eCommerce.Models.Models
{
    public class PaymentRange
    {
        public PaymentRange() { }

        [Key]
        public int PaymentRangeId { get; set; }
        public int PaymentRangeLinkId { get; set; }
        public int MinimumValue { get; set; }
        public int MaximumValue { get; set; }
        public int PercentageFees { get; set; }
    }
}
