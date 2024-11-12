using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerce.Models.ControllerModels
{
    [NotMapped]
    public class SupplierPaymentDetail
    {
        public string UserOrderId { get; set; }
        public string SupplierPhoneNo { get; set; }
        public string Price { get; set; }
        public string OrderDate { get; set; }
        public string DeliveryDate { get; set; }
        public string DeliveryStatus { get; set; }
        public string CompanyPaymentStatus { get; set; }
        public string CompanyPayment { get; set; }
        public string CompanyPaymentDate { get; set; }
        public string SupplierPaymentStatus { get; set; }
        public string SupplierPayment { get; set; }
        public string SupplierPaymentDate { get; set; }
        public string TransactionId { get; set; }
        public string Comments { get; set; }
        public string Miscellenous { get; set; }
    }
}
