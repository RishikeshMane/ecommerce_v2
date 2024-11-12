using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerce.Models.ControllerModels
{
    [NotMapped]
    public class ProductComments
    {
        public string UserProductId { get; set; }
        public string UserId { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Comment { get; set; }
        public int Rating { get; set; }
        public string Date { get; set; }
    }
}
