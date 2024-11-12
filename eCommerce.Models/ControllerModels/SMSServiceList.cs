using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerce.Models.ControllerModels
{
    [NotMapped]
    public class SMSServiceList
    {
        public List<SMSServiceDetail> SMSServiceDetail { get; set; }
    }

    [NotMapped]
    public class SMSServiceDetail
    {
        public int SMSServiceLinkId { get; set; }
        public string SMSServiceName { get; set; }
        public int ActiveNow { get; set; }
    }
}
