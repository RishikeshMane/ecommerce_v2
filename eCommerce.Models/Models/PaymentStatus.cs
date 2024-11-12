using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace eCommerce.Models.Models
{
    public class PaymentStatus
    {
        public PaymentStatus() { }

        [Key]
        public int PaymentStatusId { get; set; }
        public int PaymentStatusLinkId { get; set; }
        public string PaymentStatusCode { get; set; }
    }
}
