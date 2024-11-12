using eCommerce.DataAccess.Repository.IRepository;
using eCommerce.Models.ControllerModels;
using eCommerce.Models.Models;
using eCommerce.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using eCommerce.UserManagement.Utils;

///alter table yourTableName AUTO_INCREMENT=1;
///truncate table yourTableName;

namespace eCommerce.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public PaymentController(IUnitOfWork unitOfWork/*,
                                    HttpContextAccessor httpContextAccessor*/)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        [Route("UpdatePaymentStatus")]
        public IActionResult UpdatePaymentStatus(PaymentStatusList payments)
        {
            var createdResource = new { Id = 1, Version = "1.0" };
            string actionName = "UpdatePaymentStatus";
            var routeValues = new { id = createdResource.Id, version = createdResource.Version };

            if (ModelState.IsValid)
            {
                IEnumerable<PaymentStatus> paymentsStatusAll = _unitOfWork.PaymentStatusRepository.GetAll();
                if (paymentsStatusAll.Count() == 0)
                {
                    foreach (var paymentsObj in payments.PaymentStatusDetail)
                    {
                        PaymentStatus paymentStatus = new PaymentStatus();
                        paymentStatus.PaymentStatusLinkId = paymentsObj.PaymentStatusLinkId;
                        paymentStatus.PaymentStatusCode = paymentsObj.PaymentStatus;

                        _unitOfWork.PaymentStatusRepository.Add(paymentStatus);
                    }

                    _unitOfWork.Save();
                }
            }

            return CreatedAtAction(actionName, new { id = "success" });
        }

        [HttpPost]
        [Route("UpdatePaymentRange")]
        public IActionResult UpdatePaymentRange(PaymentRangeList paymentsRange)
        {
            var createdResource = new { Id = 1, Version = "1.0" };
            string actionName = "UpdatePaymentRange";
            var routeValues = new { id = createdResource.Id, version = createdResource.Version };

            if (ModelState.IsValid)
            {
                IEnumerable<PaymentRange> paymentsRangeAll = _unitOfWork.PaymentRangeRepository.GetAll();
                if (paymentsRangeAll.Count() == 0)
                {
                    foreach (var paymentsRangeObj in paymentsRange.PaymentRangeDetail)
                    {
                        PaymentRange paymentRange = new PaymentRange();
                        paymentRange.PaymentRangeLinkId = paymentsRangeObj.PaymentRangeLinkId;
                        paymentRange.MinimumValue = paymentsRangeObj.MinimumValue;
                        paymentRange.MaximumValue = paymentsRangeObj.MaximumValue;
                        paymentRange.PercentageFees = paymentsRangeObj.PercentageFees;

                        _unitOfWork.PaymentRangeRepository.Add(paymentRange);
                    }

                    _unitOfWork.Save();
                }
            }

            return CreatedAtAction(actionName, new { id = "success" });
        }

        [HttpGet]
        [Route("GetCompanyPayments")]
        public SupplierPaymentDetail[] GetCompanyPayments([FromQuery] string mobileNo, [FromQuery] string fromDate, [FromQuery] string toDate, [FromQuery] string onlyCOD)
        {
            var createdResource = new { Id = 1, Version = "1.0" };
            string actionName = "GetCompanyPayments";
            var routeValues = new { id = createdResource.Id, version = createdResource.Version };

            SupplierPaymentDetail[] supplierPaymentDetails = new SupplierPaymentDetail[] { };

            if (ModelState.IsValid)
            {
                DateTime from = new DateTime(ToYear(fromDate), ToMonth(fromDate), ToDay(fromDate));
                DateTime to = new DateTime(ToYear(toDate), ToMonth(toDate), ToDay(toDate));

                IEnumerable<OrderHeader> orderHeaders = _unitOfWork.OrderHeaderRepository.GetAll(r => (r.OrderDate >= from && r.OrderDate <= to));
                IList<string> ordersList = GetOrdersNos(orderHeaders);
                IEnumerable<OrderDetails> orderDetails = new List<OrderDetails>();
                if (mobileNo == "All")
                    orderDetails = _unitOfWork.OrderDetailsRepository.GetAll(r=> (ordersList.Contains(r.UserOrderId)));
                else
                    orderDetails = _unitOfWork.OrderDetailsRepository.GetAll(r => (ordersList.Contains(r.UserOrderId) &&
                                                                                                       r.SupplierMobileNo == long.Parse(mobileNo)));

                if (orderDetails.Count() == 0)
                    return supplierPaymentDetails;

                IEnumerable<OrderStatus> orderStatus = _unitOfWork.OrderStatusRepository.GetAll();
                IEnumerable<Payments> payments = _unitOfWork.PaymentsRepository.GetAll();
                IEnumerable<PaymentStatus> paymentStatuses = _unitOfWork.PaymentStatusRepository.GetAll();

                var orders = orderDetails.GroupBy(r => new { r.UserOrderId, r.SupplierMobileNo })
                                        .Select(g => new
                                        {
                                            UserOrderId = g.Key.UserOrderId,
                                            SupplierMobileNo = g.Key.SupplierMobileNo,
                                            price = g.Sum(g => (g.Price * g.Quantity)),
                                            orderStatusLinkId = g.Min(g => g.OrderStatusLinkId)
                                        });

                foreach(var order in orders)
                {
                    if (onlyCOD == "true" && order.UserOrderId.IndexOf("COD-") != 0)
                        continue;

                    SupplierPaymentDetail supplierPaymentDetail = new SupplierPaymentDetail();

                    OrderHeader? orderHeader = orderHeaders.Where(r => (r.UserOrderId == order.UserOrderId))
                                            .FirstOrDefault();

                    Payments? paymentRecord = payments.Where(r => (r.UserOrderId == order.UserOrderId && r.SupplierMobileNo == order.SupplierMobileNo))
                                            .FirstOrDefault();

                    supplierPaymentDetail.UserOrderId = order.UserOrderId;
                    supplierPaymentDetail.SupplierPhoneNo = order.SupplierMobileNo.ToString();
                    supplierPaymentDetail.Price = order.price.ToString();
                    supplierPaymentDetail.DeliveryStatus = orderStatus.FirstOrDefault(r => r.OrderStatusLinkId == order.orderStatusLinkId).Description;
                    supplierPaymentDetail.OrderDate = orderHeader.OrderDate.ToString();

                    supplierPaymentDetail.CompanyPayment = paymentRecord == null ? "0" : paymentRecord.CompanyPayment.ToString();
                    supplierPaymentDetail.CompanyPaymentDate = paymentRecord == null ? "0001-01-01 00:00:00" : paymentRecord.CompanyPaymentDate.ToString();
                    supplierPaymentDetail.CompanyPaymentStatus = paymentRecord == null ? "Unsettled" : paymentStatuses.FirstOrDefault(r => r.PaymentStatusLinkId == ((paymentRecord.CompanyPaymenStatus == 0) ? 1 : paymentRecord.CompanyPaymenStatus)).PaymentStatusCode;
                    supplierPaymentDetail.SupplierPayment = paymentRecord == null ? "0" : paymentRecord.SupplierPayment.ToString();
                    supplierPaymentDetail.SupplierPaymentDate = paymentRecord == null ? "0001-01-01 00:00:00" : paymentRecord.SupplierPaymentDate.ToString();
                    supplierPaymentDetail.SupplierPaymentStatus = paymentRecord == null ? "Unsettled" : paymentStatuses.FirstOrDefault(r => r.PaymentStatusLinkId == ((paymentRecord.SupplierPaymenStatus == 0) ? 1 : paymentRecord.SupplierPaymenStatus)).PaymentStatusCode;
                    supplierPaymentDetail.TransactionId = paymentRecord == null ? "" : paymentRecord.SupplierCompanyTransactionId;
                    supplierPaymentDetail.Comments = paymentRecord == null ? "" : paymentRecord.Comments;

                    supplierPaymentDetails = supplierPaymentDetails.Concat(new SupplierPaymentDetail[] { supplierPaymentDetail }).ToArray();
                }

            }

            return supplierPaymentDetails;
        }
        
        [HttpGet]
        [Route("GetSupplierPayments")]
        public SupplierPaymentDetail[] GetSupplierPayments([FromQuery] string mobileNo, [FromQuery] string fromDate, [FromQuery] string toDate, [FromQuery] string onlyCOD)
        {
            var createdResource = new { Id = 1, Version = "1.0" };
            string actionName = "GetSupplierPayments";
            var routeValues = new { id = createdResource.Id, version = createdResource.Version };

            SupplierPaymentDetail[] supplierPaymentDetails = new SupplierPaymentDetail[] { };

            if (ModelState.IsValid)
            {
                IEnumerable<OrderDetails> orderDetails = _unitOfWork.OrderDetailsRepository.GetAll(r => (r.SupplierMobileNo == long.Parse(mobileNo)));

                OrderDetails orderDetail = orderDetails.FirstOrDefault(r => (r.SupplierMobileNo == long.Parse(mobileNo)));
                if (orderDetail == null)
                    return supplierPaymentDetails;

                IEnumerable<Payments> payments = _unitOfWork.PaymentsRepository.GetAll(r => (r.SupplierMobileNo == long.Parse(mobileNo)));

                IList<string> orders = GetOrdersNos(orderDetails);

                string userOrderId = orderDetail.UserOrderId;
                IEnumerable<OrderHeader> orderHeaders = _unitOfWork.OrderHeaderRepository.GetAll(r => (orders.Contains(r.UserOrderId)));
                IEnumerable<OrderStatus> orderStatuses = _unitOfWork.OrderStatusRepository.GetAll();
                IEnumerable<PaymentStatus> paymentStatuses = _unitOfWork.PaymentStatusRepository.GetAll();

                foreach (var orderHeader in orderHeaders)
                {
                    DateTime from = new DateTime(ToYear(fromDate), ToMonth(fromDate), ToDay(fromDate));
                    DateTime to = new DateTime(ToYear(toDate), ToMonth(toDate), ToDay(toDate));
                    bool isOrderCancel = false;

                    if (from.CompareTo(orderHeader.OrderDate) > 0 || to.CompareTo(orderHeader.OrderDate) < 0)
                        continue;

                    if (onlyCOD == "true" && orderHeader.UserOrderId.IndexOf("COD-") != 0)
                        continue;

                    SupplierPaymentDetail supplierPaymentDetail = new SupplierPaymentDetail();
                    supplierPaymentDetail.UserOrderId = orderHeader.UserOrderId;
                    supplierPaymentDetail.OrderDate = orderHeader.OrderDate.ToString();

                    int payment = 0;
                    int minOrderStatus = 3;
                    string minOrderDeliveryDate = "01-01-0001 00:00:00";
                    foreach(var orderDetaill in orderDetails)
                    {
                        if (supplierPaymentDetail.UserOrderId == orderDetaill.UserOrderId &&
                            orderDetaill.SupplierMobileNo == long.Parse(mobileNo))
                        {
                            payment += (orderDetaill.Price * orderDetaill.Quantity);

                            if (orderDetaill.OrderStatusLinkId < minOrderStatus)
                            {
                                minOrderStatus = orderDetaill.OrderStatusLinkId;
                                minOrderDeliveryDate = orderDetaill.DeliveryDate.ToString();
                            }

                            /// Order is cancelled
                            if (orderDetaill.OrderStatusLinkId > 3)
                                isOrderCancel = true;
                        }
                    }

                    if (isOrderCancel)
                        continue;

                    supplierPaymentDetail.Price = payment.ToString();

                    Payments? paymentRecord = payments.FirstOrDefault(r => r.UserOrderId == orderHeader.UserOrderId &&
                            r.SupplierMobileNo == long.Parse(mobileNo));

                    supplierPaymentDetail.CompanyPayment = paymentRecord == null ? "0" : paymentRecord.CompanyPayment.ToString();
                    supplierPaymentDetail.CompanyPaymentDate = paymentRecord == null ? "0001-01-01 00:00:00" : paymentRecord.CompanyPaymentDate.ToString();
                    supplierPaymentDetail.CompanyPaymentStatus = paymentRecord == null ? "Unsettled" : paymentStatuses.FirstOrDefault(r => r.PaymentStatusLinkId == ((paymentRecord.CompanyPaymenStatus == 0) ? 1 : paymentRecord.CompanyPaymenStatus)).PaymentStatusCode;
                    supplierPaymentDetail.SupplierPayment = paymentRecord == null ? "0" : paymentRecord.SupplierPayment.ToString();
                    supplierPaymentDetail.SupplierPaymentDate = paymentRecord == null ? "0001-01-01 00:00:00" : paymentRecord.SupplierPaymentDate.ToString();
                    supplierPaymentDetail.SupplierPaymentStatus = paymentRecord == null ? "Unsettled" : paymentStatuses.FirstOrDefault(r => r.PaymentStatusLinkId == ((paymentRecord.SupplierPaymenStatus == 0) ? 1 : paymentRecord.SupplierPaymenStatus)).PaymentStatusCode;
                    supplierPaymentDetail.TransactionId = paymentRecord == null ? "" : paymentRecord.SupplierCompanyTransactionId;
                    supplierPaymentDetail.Comments = paymentRecord == null ? "" : paymentRecord.Comments;

                    supplierPaymentDetail.DeliveryStatus = orderStatuses.FirstOrDefault(r => r.OrderStatusLinkId == minOrderStatus).Description;
                    supplierPaymentDetail.DeliveryDate = minOrderDeliveryDate;

                    supplierPaymentDetails = supplierPaymentDetails.Concat(new SupplierPaymentDetail[] { supplierPaymentDetail }).ToArray();
                }
            }

            return supplierPaymentDetails;
        }

        [HttpPost]
        [Route("UpdatePaymentsSupplier")]
        public IActionResult UpdatePaymentsSupplier(string userOrderId, string mobileno, string price, string paymentStatusLinkId, string payment,
                                            string updatedPaymentDate, string transactionId, string comments)
        {
            string actionName = "UpdatePaymentsSupplier";

            if (ModelState.IsValid)
            {
                if (transactionId == "TX") transactionId = "";
                if (comments == "CO") transactionId = "";

                bool bAddUpdate = false;

                ///OrderHeader orderHeader = _unitOfWork.OrderHeaderRepository.GetFirstOrDefault( r => r.UserOrderId == userOrderId);
                Payments paymentRecord = _unitOfWork.PaymentsRepository.GetFirstOrDefault(r => (r.UserOrderId == userOrderId && r.SupplierMobileNo == long.Parse(mobileno)));
                if (paymentRecord == null)
                {
                    bAddUpdate = true;
                    paymentRecord = new Payments();
                    paymentRecord.UserOrderId = userOrderId;
                    paymentRecord.SupplierMobileNo = long.Parse(mobileno);
                    paymentRecord.Price = int.Parse(price);
                }

                IEnumerable<PaymentStatus> paymentStatus = _unitOfWork.PaymentStatusRepository.GetAll();

                if (userOrderId.IndexOf("COD-") == 0)
                {
                    paymentRecord.CompanyPayment = "";
                    paymentRecord.SupplierPayment = payment;
                    paymentRecord.SupplierPaymenStatus = int.Parse(paymentStatusLinkId);
                    if (updatedPaymentDate != "0000-00-00" && updatedPaymentDate != "0001-01-01")
                    {
                        paymentRecord.SupplierPaymentDate = new DateTime(ToYear(updatedPaymentDate), ToMonth(updatedPaymentDate), ToDay(updatedPaymentDate));
                    }
                }
                else
                {
                    paymentRecord.SupplierPayment = "";
                    paymentRecord.CompanyPayment = payment;
                    paymentRecord.CompanyPaymenStatus = int.Parse(paymentStatusLinkId);
                    if (updatedPaymentDate != "0000-00-00" && updatedPaymentDate != "0001-01-01")
                    {
                        paymentRecord.CompanyPaymentDate = new DateTime(ToYear(updatedPaymentDate), ToMonth(updatedPaymentDate), ToDay(updatedPaymentDate));
                    }
                }

                paymentRecord.SupplierCompanyTransactionId = transactionId;
                paymentRecord.Comments = comments;

                if (bAddUpdate)
                    _unitOfWork.PaymentsRepository.Add(paymentRecord);
                else
                    _unitOfWork.PaymentsRepository.Update(paymentRecord);

                _unitOfWork.Save();
            }

            return CreatedAtAction(actionName, new { });
        }

        [HttpGet]
        [Route("GetSupplierInfo")]
        public SupplierInfo[] GetSupplierInfo()
        {
            var createdResource = new { Id = 1, Version = "1.0" };
            string actionName = "GetSupplierInfo";
            var routeValues = new { id = createdResource.Id, version = createdResource.Version };

            SupplierInfo[] supplierInfo = new SupplierInfo[] { };

            if (ModelState.IsValid)
            {
                IEnumerable<User> users = _unitOfWork.UserRepository.GetAll();
                IEnumerable<City> cities = _unitOfWork.CityRepository.GetAll();

                foreach(var user in users)
                {
                    if (user.UserRoleId == 3)
                    {
                        SupplierInfo supplierDetails = new SupplierInfo();

                        supplierDetails.MobileNo = user.Phone1.ToString();
                        /// Country hardcoded to India
                        supplierDetails.City = cities.FirstOrDefault(r => ( r.CountryLinkId == 1 && r.CityLinkId == user.CityId)).Name;
                        supplierDetails.StoreName = user.StoreName;

                        supplierInfo = supplierInfo.Concat(new SupplierInfo[] { supplierDetails }).ToArray();
                    }
                }
            }

            return supplierInfo;
        }

        private IList<string> GetOrdersNos(IEnumerable<OrderDetails> orderDetails)
        {
            IList<string> orders = new List<string>();

            foreach (var orderDetail in orderDetails)
            {
                if (!orders.Contains(orderDetail.UserOrderId))
                    orders.Add(orderDetail.UserOrderId);
            }

            return orders;
        }

        private IList<string> GetOrdersNos(IEnumerable<OrderHeader> orderHeaders)
        {
            IList<string> orders = new List<string>();

            foreach (var orderHeader in orderHeaders)
            {
                if (!orders.Contains(orderHeader.UserOrderId))
                    orders.Add(orderHeader.UserOrderId);
            }

            return orders;
        }

        private int ToYear(string fromDate)
        {
            return int.Parse(fromDate.Substring(0, 4));
        }

        private int ToMonth(string fromDate)
        {
            return int.Parse(fromDate.Substring(5, 2));
        }

        private int ToDay(string fromDate)
        {
            return int.Parse(fromDate.Substring(8, 2));
        }

    }
}