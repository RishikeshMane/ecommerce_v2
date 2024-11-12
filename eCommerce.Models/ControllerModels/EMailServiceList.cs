using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerce.Models.ControllerModels
{
    [NotMapped]
    public class EMailServiceList
    {
        public List<EMailServiceDetail> EMailServiceDetail { get; set; }
    }

    [NotMapped]
    public class EMailServiceDetail
    {
        public int EMailServiceLinkId { get; set; }
        public string EMailServiceName { get; set; }
        public int ActiveNow { get; set; }
    }
}
