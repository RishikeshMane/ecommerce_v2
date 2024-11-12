using eCommerce.DataAccess.Repository.IRepository;
using eCommerce.Models.ControllerModels;
using eCommerce.Models.Models;
using eCommerce.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using eCommerce.UserManagement.Utils;
using Razorpay.Api;
using System.Text.Json.Nodes;

///alter table yourTableName AUTO_INCREMENT=1;
///truncate table yourTableName;

namespace eCommerce.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderController(IUnitOfWork unitOfWork/*,
                                    HttpContextAccessor httpContextAccessor*/)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("GetOrderNo")]
        public IActionResult GetOrderNo([FromQuery] string mobileNo, [FromQuery] string summaryTotal)
        {
            string actionName = "GetOrderNo";

            string orderId = "order_NjrE1fNUC7wByM";
            string razorPayKey = "";
            string razorPayKeySecret = "";

            float total = int.Parse(summaryTotal) + ProductUtils.GetHandlingChanges();
            total += total * (float)(ProductUtils.GetPercentageFees() / 100.0);
            summaryTotal = ((int)Math.Ceiling(total)).ToString();
            
            try
            {
                if (ModelState.IsValid)
                {
                    IEnumerable<eCommerce.Models.Models.Razorpay> razorPays = _unitOfWork.RazorpayRepository.GetAll();

                    foreach (var razorPay in razorPays)
                    {
                        razorPayKey = razorPay.KeyId;
                        razorPayKeySecret = razorPay.KeySecret;
                    }
                }

                RazorpayClient razorpayClient = new RazorpayClient(razorPayKey, razorPayKeySecret);

                Dictionary<string, object> orderOptions = new Dictionary<string, object>();
                orderOptions.Add("amount", long.Parse(summaryTotal) * 100);
                orderOptions.Add("currency", "INR");

                ///Razorpay.Api.Order order = razorpayClient.Order.Create(orderOptions);

                ///orderId = order["id"];
            }
            catch (Exception ex)
            {

            }

            return CreatedAtAction(actionName, new { orderId = orderId, razorPayKey = razorPayKey, razorPayKeySecret = razorPayKeySecret });
        }

        [HttpGet]
        [Route("GetPaymentGateway")]
        public IActionResult GetPaymentGateway()
        {
            string actionName = "GetPaymentGateway";

            string paymentGatewayName = "RazorPay";

            if (ModelState.IsValid)
            {
                IEnumerable<PaymentGateway> paymentGateways = _unitOfWork.PaymentGatewayRepository.GetAll();

                foreach (var paymentGateway in paymentGateways)
                {
                    if (paymentGateway.ActiveNow == 1)
                    {
                        paymentGatewayName = paymentGateway.PaymentGatewayName;
                    }
                }
            }

            return CreatedAtAction(actionName, new { id = paymentGatewayName });
        }

        [HttpGet]
        [Route("GetSMSService")]
        public IActionResult GetSMSService()
        {
            string actionName = "GetSMSService";

            string smsServiceName = "FAST2SMS";

            string fast2smsKey = "";

            if (ModelState.IsValid)
            {
                IEnumerable<SMSService> SMSServices = _unitOfWork.SMSServiceRepository.GetAll();

                foreach (var SMSService in SMSServices)
                {
                    if (SMSService.ActiveNow == 1)
                    {
                        smsServiceName = SMSService.SMSServiceName;
                    }
                }

                IEnumerable<eCommerce.Models.Models.FAST2SMS> FAST2SMSES = _unitOfWork.FAST2SMSRepository.GetAll();

                foreach (var FAST2SMS in FAST2SMSES)
                {
                    fast2smsKey = FAST2SMS.KeyId;
                }
            }

            if (smsServiceName == "FAST2SMS")
                return CreatedAtAction(actionName, new { id = smsServiceName, fast2smsKey = fast2smsKey });

            return CreatedAtAction(actionName, new { id = smsServiceName });
        }

        [HttpGet]
        [Route("GetEMailService")]
        public IActionResult GetEMailService()
        {
            string actionName = "GetEMailService";

            string emailServiceName = "EMailJS";

            string eMailJSKeyId = "";
            string eMailJSServiceId = "";
            string eMailJSTemplateId = "";

            if (ModelState.IsValid)
            {
                IEnumerable<EMailService> EMailServices = _unitOfWork.EMailServiceRepository.GetAll();

                foreach (var EMailService in EMailServices)
                {
                    if (EMailService.ActiveNow == 1)
                    {
                        emailServiceName = EMailService.EMailServiceName;
                    }
                }

                IEnumerable<eCommerce.Models.Models.EMailJS> eMailJSes = _unitOfWork.EMailJSRepository.GetAll();

                foreach (var eMailJS in eMailJSes)
                {
                    eMailJSKeyId = eMailJS.KeyId;
                    eMailJSServiceId = eMailJS.ServiceId;
                    eMailJSTemplateId = eMailJS.TemplateId;
                }
            }

            if (emailServiceName == "EMailJS")
                return CreatedAtAction(actionName, new { id = emailServiceName, eMailJSKeyId = eMailJSKeyId, eMailJSServiceId = eMailJSServiceId, eMailJSTemplateId = eMailJSTemplateId });

            return CreatedAtAction(actionName, new { id = emailServiceName });
        }

        [HttpGet]
        [Route("GetOrders")]
        public UserOrderDetail[] GetOrders([FromQuery] string mobileNo, [FromQuery] string fromDate, [FromQuery] string toDate)
        {
            UserOrderDetail[] userOrderDetails = new UserOrderDetail[] { };

            if (ModelState.IsValid)
            {
                IEnumerable<OrderDetails> orderDetails = _unitOfWork.OrderDetailsRepository.GetAll(r => (r.SupplierMobileNo == long.Parse(mobileNo)));

                OrderDetails orderDetail = orderDetails.FirstOrDefault(r => (r.SupplierMobileNo == long.Parse(mobileNo)));
                if (orderDetail == null)
                    return userOrderDetails;

                IList<string> orders = GetOrdersNos(orderDetails);

                string userOrderId = orderDetail.UserOrderId;
                IEnumerable<OrderHeader> orderHeaders = _unitOfWork.OrderHeaderRepository.GetAll(r => (orders.Contains(r.UserOrderId)));
                IEnumerable<OrderStatus> orderStatuses = _unitOfWork.OrderStatusRepository.GetAll();

                IEnumerable<Size> sizes = _unitOfWork.SizeRepository.GetAll();
                IEnumerable<Color> colors = _unitOfWork.ColorRepository.GetAll();
                IEnumerable<Country> countries = _unitOfWork.CountryRepository.GetAll();
                IEnumerable<State> states = _unitOfWork.StateRepository.GetAll();
                IEnumerable<City> cities = _unitOfWork.CityRepository.GetAll();

                foreach (var orderDetaill in orderDetails)
                {
                    DateTime from = new DateTime(ToYear(fromDate), ToMonth(fromDate), ToDay(fromDate));
                    DateTime to = new DateTime(ToYear(toDate), ToMonth(toDate), ToDay(toDate));
                    OrderHeader? orderHeader = orderHeaders.FirstOrDefault(r => (r.UserOrderId == orderDetaill.UserOrderId));

                    if (from.CompareTo(orderHeader.OrderDate) > 0 || to.CompareTo(orderHeader.OrderDate) < 0)
                        continue;

                    /// Order Cancel or Refunded
                    if (orderDetaill.OrderStatusLinkId == 4 || orderDetaill.OrderStatusLinkId == 5)
                        continue;

                    Size? size = sizes.FirstOrDefault(r => (r.SizeLinkId == orderDetaill.SizeLinkId));
                    Color? color = colors.FirstOrDefault(r => (r.ColorLinkId == orderDetaill.ColorLinkId));

                    Country? country = countries.FirstOrDefault(r => (r.CountryLinkId == orderHeader.CountryId));
                    State? state = states.FirstOrDefault(r => (r.CountryLinkId == orderHeader.CountryId &&
                                                                                        r.StateLinkId == orderHeader.StateId));
                    City? city = cities.FirstOrDefault(r => (r.CountryLinkId == orderHeader.CountryId &&
                                                                                        r.StateLinkId == orderHeader.StateId &&
                                                                                        r.CityLinkId == orderHeader.CityId));

                    UserOrderDetail userOrderDetail = new UserOrderDetail();
                    userOrderDetail.UserOrderId = orderDetaill.UserOrderId;
                    userOrderDetail.UserProductId = orderDetaill.UserProductId;
                    userOrderDetail.Title = orderDetaill.Title;
                    userOrderDetail.Address = getAddress(orderHeader, country.Name, state.Name, city.Name);
                    userOrderDetail.Color = color.Description;
                    userOrderDetail.Size = size.SizeCode;
                    userOrderDetail.Price = orderDetaill.Price.ToString();
                    userOrderDetail.Count = orderDetaill.Quantity.ToString();
                    userOrderDetail.OrderDate = orderHeader.OrderDate.ToString();
                    userOrderDetail.DeliveryDate = orderDetaill.DeliveryDate.ToString();
                    OrderStatus? orderStatus = orderStatuses.FirstOrDefault(r => r.OrderStatusLinkId == orderDetaill.OrderStatusLinkId);
                    userOrderDetail.DeliveryStatus = orderStatus.Description;

                    userOrderDetails = userOrderDetails.Concat(new UserOrderDetail[] { userOrderDetail }).ToArray();
                }
            }

            return userOrderDetails;
        }

        [HttpGet]
        [Route("GetOrderHistory")]
        public UserOrderHistory[] GetOrderHistory([FromQuery] string mobileNo)
        {
            UserOrderHistory[] userOrderDetails = new UserOrderHistory[] { };

            if (ModelState.IsValid)
            {
                List<UserOrderHistory> list = new List<UserOrderHistory>();

                IEnumerable<OrderHeader> orderHeaders = _unitOfWork.OrderHeaderRepository.GetAll(r => (r.MobileNo == long.Parse(mobileNo)));
                IEnumerable<Country> countries = _unitOfWork.CountryRepository.GetAll();
                IEnumerable<State> states = _unitOfWork.StateRepository.GetAll();
                IEnumerable<City> cities = _unitOfWork.CityRepository.GetAll();

                foreach (var orderHeader in orderHeaders)
                {
                    UserOrderHistory userOrderHistory = new UserOrderHistory();
                    Country? country = countries.FirstOrDefault(r => (r.CountryLinkId == orderHeader.CountryId));
                    State? state = states.FirstOrDefault(r => (r.CountryLinkId == orderHeader.CountryId &&
                                                                                        r.StateLinkId == orderHeader.StateId));
                    City? city = cities.FirstOrDefault(r => (r.CountryLinkId == orderHeader.CountryId &&
                                                                                        r.StateLinkId == orderHeader.StateId &&
                                                                                        r.CityLinkId == orderHeader.CityId));

                    userOrderHistory.UserOrderId = orderHeader.UserOrderId;
                    userOrderHistory.Address = getAddressOnly(orderHeader, country.Name, state.Name, city.Name);
                    userOrderHistory.OrderDate = orderHeader.OrderDate.ToString();

                    DateTime dateTime = DateTime.Now;
                    TimeSpan timeSpan = DateTime.Now.Subtract(orderHeader.OrderDate);

                    ///userOrderHistory.CanCancelOrder = true;
                    userOrderHistory.CanCancelOrder = false;

                    if (timeSpan.TotalDays < 1 && timeSpan.Hours < 6)
                        userOrderHistory.CanCancelOrder = true;

                    IEnumerable<OrderDetails> orderDetails = _unitOfWork.OrderDetailsRepository.GetAll(r => (r.UserOrderId == orderHeader.UserOrderId));

                    int price = 0;
                    DateTime deliveryDate = DateTime.Now;
                    int orderStatusLinkId = 5;

                    foreach (var orderDetail in orderDetails)
                    {
                        price += (orderDetail.Price * orderDetail.Quantity) ;

                        if (orderDetail.OrderStatusLinkId < orderStatusLinkId)
                            orderStatusLinkId = orderDetail.OrderStatusLinkId;

                        if (orderDetail.DeliveryDate < deliveryDate)
                            deliveryDate = orderDetail.DeliveryDate;

                        if (orderDetail.OrderStatusLinkId > 1)
                            userOrderHistory.CanCancelOrder = false;
                    }

                    userOrderHistory.Price = price.ToString();

                    if (orderStatusLinkId == 1)
                        userOrderHistory.DeliveryStatus = "Order Received";
                    else if (orderStatusLinkId == 2)
                        userOrderHistory.DeliveryStatus = "Out for Delivery";
                    else if (orderStatusLinkId == 3)
                        userOrderHistory.DeliveryStatus = "Delivered";
                    else if (orderStatusLinkId == 4)
                        userOrderHistory.DeliveryStatus = "Order Cancel";

                    userOrderHistory.DeliveryDate = deliveryDate.ToString();

                    list.Add(userOrderHistory);
                    ///userOrderDetails = userOrderDetails.Concat(new UserOrderHistory[] { userOrderHistory }).ToArray();
                }

                DateComparator comparator = new DateComparator();
                list.Sort(comparator);
                list.Reverse();

                foreach (var userOrderHistory in list)
                {
                    userOrderDetails = userOrderDetails.Concat(new UserOrderHistory[] { userOrderHistory }).ToArray();
                }
            }

            return userOrderDetails;
        }

        [HttpGet]
        [Route("GetOrderDetails")]
        public UserOrderDetails[] GetOrderDetails([FromQuery] string userOrderId)
        {
            UserOrderDetails[] userOrderDetails = new UserOrderDetails[] { };

            if (ModelState.IsValid)
            {
                IEnumerable<OrderDetails> orderDetails = _unitOfWork.OrderDetailsRepository.GetAll(r => (r.UserOrderId == userOrderId));
                IEnumerable<Size> sizes = _unitOfWork.SizeRepository.GetAll();
                IEnumerable<Color> colors = _unitOfWork.ColorRepository.GetAll();

                foreach (var orderDetail in orderDetails)
                {
                    UserOrderDetails userOrderDetail = new UserOrderDetails();

                    Size? size = sizes.FirstOrDefault(r => (r.SizeLinkId == orderDetail.SizeLinkId));
                    Color? color = colors.FirstOrDefault(r => (r.ColorLinkId == orderDetail.ColorLinkId));

                    userOrderDetail.UserOrderId = orderDetail.UserOrderId;
                    userOrderDetail.Title = orderDetail.Title;
                    userOrderDetail.Color = color.Description;
                    userOrderDetail.Size = size.SizeCode;
                    userOrderDetail.Quantity = orderDetail.Quantity.ToString();
                    userOrderDetail.Price = orderDetail.Price.ToString();

                    userOrderDetails = userOrderDetails.Concat(new UserOrderDetails[] { userOrderDetail }).ToArray();
                }
            }

            return userOrderDetails;
        }

        [HttpPost]
        [Route("CancelOrder")]
        public IActionResult CancelOrder(string userOrderId)
        {
            string actionName = "CancelOrder";
            string status = "Ok";

            if (ModelState.IsValid)
            {
                OrderStatus orderStatus = _unitOfWork.OrderStatusRepository.GetFirstOrDefault(r => (r.Description == "Order Cancel"));
                
                IEnumerable<OrderDetails> orderDetails = _unitOfWork.OrderDetailsRepository.GetAll(r => (r.UserOrderId == userOrderId));

                foreach(var orderDetail in orderDetails)
                {
                    orderDetail.OrderStatusLinkId = orderStatus.OrderStatusLinkId;

                    _unitOfWork.OrderDetailsRepository.Update(orderDetail);
                    _unitOfWork.Save();
                }
            }

            return CreatedAtAction(actionName, new { id = status });
        }

        [HttpPost]
        [Route("AddOrder")]
        public IActionResult AddOrder(ShoppingCartDetails shoppingCart)
        {
            string actionName = "AddOrder";
            string status = "Ok";

            if (ModelState.IsValid)
            {
                string gateway = getPaymentGateway(shoppingCart.OrderId);

                User user = _unitOfWork.UserRepository.GetFirstOrDefault(r => (r.Phone1 == long.Parse(shoppingCart.MobileNo)));
                ///IList<Address> addresses = _unitOfWork.AddressRepository.GetAll().Where(r => (r.PhoneNo == long.Parse(shoppingCart.MobileNo))).ToList();
                IList<Address> addresses = _unitOfWork.AddressRepository.GetAll(r => (r.PhoneNo == long.Parse(shoppingCart.MobileNo))).ToList();
                OrderStatus orderStatus = _unitOfWork.OrderStatusRepository.GetFirstOrDefault(r => (r.Description == "Order Received"));
                PaymentGateway paymentGateway = _unitOfWork.PaymentGatewayRepository.GetFirstOrDefault(r => (r.PaymentGatewayName == gateway));

                Address address = getShippingAddress(user, addresses, shoppingCart.ShoppingAddressIndex);

                OrderHeader orderHeader = new OrderHeader();

                orderHeader.OrderId = "";
                orderHeader.UserOrderId = shoppingCart.OrderId;
                orderHeader.PaymentId = shoppingCart.PaymentId;
                orderHeader.MobileNo = long.Parse(shoppingCart.MobileNo);
                orderHeader.CountryId = address.CountryId;
                orderHeader.StateId = address.StateId;
                orderHeader.CityId = address.CityId;
                orderHeader.Pincode = address.PinCode;
                orderHeader.Address = address.Addresss;
                orderHeader.FirstName = user.FirstName;
                string lastName = "";
                if (user.LastName != null) lastName = user.LastName;
                orderHeader.LastName = lastName;
                string email = "";
                if (user.Email != null) email = user.Email;
                orderHeader.EmailId = email;
                orderHeader.OrderDate = DateTime.Now;
                orderHeader.Comments = "";
                orderHeader.GatewayLinkId = "";
                if (gateway.Length > 0) orderHeader.GatewayLinkId = paymentGateway.PaymentGatewayLinkId;
                orderHeader.PaymentId = "";
                orderHeader.ReceiptId = "";
                orderHeader.TransactionId = "";

                _unitOfWork.OrderHeaderRepository.Add(orderHeader);

                _unitOfWork.Save();

                foreach (var productDetail in shoppingCart.ProductDetail)
                {
                    OrderDetails orderDetails = new OrderDetails();

                    ProductVariables productVariables = _unitOfWork.ProductVariablesRepository.GetFirstOrDefault(r => r.UserProductId == productDetail.UserProductId &&
                                                                                                                    r.SizeLinkId == productDetail.SizeLinkId &&
                                                                                                                    r.ColorLinkId == productDetail.ColorLinkId);

                    if (!CheckDelivery(address.PinCode.ToString(), productVariables.UserProductId))
                    {
                        orderDetails.Quantity = 0;
                        status = "Out of Stock";
                    }
                    else
                    {
                        if (productVariables.Inventory - productDetail.Count < 0)
                        {
                            orderDetails.Quantity = productVariables.Inventory;
                            productVariables.Inventory = 0;
                            status = "Out of Stock";
                        }
                        else
                        {
                            orderDetails.Quantity = productDetail.Count;
                            productVariables.Inventory = productVariables.Inventory - productDetail.Count;
                        }
                    }

                    _unitOfWork.ProductVariablesRepository.Update(productVariables);
                    
                    _unitOfWork.Save();

                    orderDetails.UserProductId = productDetail.UserProductId;
                    orderDetails.UserOrderId = shoppingCart.OrderId;
                    orderDetails.Title = productDetail.Title;
                    orderDetails.SizeLinkId = productDetail.SizeLinkId;
                    orderDetails.ColorLinkId = productDetail.ColorLinkId;
                    orderDetails.Price = productDetail.Price;
                    orderDetails.SupplierMobileNo = long.Parse(productDetail.SupplierMobileNo);
                    orderDetails.OrderStatusLinkId = 1; /// orderStatus.OrderStatusLinkId;

                    _unitOfWork.OrderDetailsRepository.Add(orderDetails);

                    _unitOfWork.Save();
                }

                if (shoppingCart.ClearShoppingCart == 1)
                { 
                    IEnumerable<ShoppingCart> shoppingCarts = _unitOfWork.ShoppingCartRepository.GetAll(r => (r.MobileNo == long.Parse(shoppingCart.MobileNo)));

                    _unitOfWork.ShoppingCartRepository.RemoveRange(shoppingCarts);

                    _unitOfWork.Save();
                }
            }

            return CreatedAtAction(actionName, new {id = status});
        }

        [HttpPost]
        [Route("CheckAvailabilityAndTotalPayment")]
        public IActionResult CheckAvailabilityAndTotalPayment(ShoppingCartDetails shoppingCart)
        {
            string actionName = "CheckAvailabilityAndTotalPayment";

            int summaryTotal = 0;

            if (ModelState.IsValid)
            { 
                foreach (var productDetail in shoppingCart.ProductDetail)
                {
                    ProductVariables productVariables = _unitOfWork.ProductVariablesRepository.GetFirstOrDefault(r => r.UserProductId == productDetail.UserProductId &&
                                                                                                                    r.SizeLinkId == productDetail.SizeLinkId &&
                                                                                                                    r.ColorLinkId == productDetail.ColorLinkId);
                    int productCount = 0;
                    if (productVariables.Inventory - productDetail.Count < 0)
                        productCount = productVariables.Inventory;
                    else
                        productCount = productDetail.Count;

                    summaryTotal += (productCount * productDetail.Price);
                }

            }

            return CreatedAtAction(actionName, new { summaryTotal = summaryTotal.ToString() });
        }

        [HttpPost]
        [Route("UpdateOrderStatus")]
        public IActionResult UpdateOrderStatus(string userOrderId, string productId, string orderStatusLinkId, string updatedDeliveryDate)
        {
            string actionName = "UpdateOrderStatus";

            if (ModelState.IsValid)
            {
                OrderDetails orderDetail = _unitOfWork.OrderDetailsRepository.GetFirstOrDefault(r => (r.UserOrderId == userOrderId &&
                                                                                                        r.UserProductId == productId));
                orderDetail.OrderStatusLinkId = int.Parse(orderStatusLinkId);

                if (ToYear(updatedDeliveryDate) != 0)
                {
                    orderDetail.DeliveryDate = new DateTime(ToYear(updatedDeliveryDate), ToMonth(updatedDeliveryDate), ToDay(updatedDeliveryDate));
                }

                _unitOfWork.OrderDetailsRepository.Update(orderDetail);

                _unitOfWork.Save();
            }

            return CreatedAtAction(actionName, new { });
        }

        [HttpGet]
        [Route("GetOrderStatus")]
        public UserOrderDetail[] GetOrderStatus([FromQuery] string userOrderId)
        {
            UserOrderDetail[] userOrderDetails = new UserOrderDetail[] { };

            if (ModelState.IsValid)
            {
                IList<OrderDetails> orderDetails = _unitOfWork.OrderDetailsRepository.GetAll(r => (r.UserOrderId == userOrderId)).ToList();
                IEnumerable<OrderStatus> orderStatus = _unitOfWork.OrderStatusRepository.GetAll();

                foreach (var orderDetail in orderDetails)
                {
                    UserOrderDetail userOrderDetail = new UserOrderDetail();

                    userOrderDetail.UserProductId = orderDetail.UserProductId;
                    userOrderDetail.Title = orderDetail.Title;
                    userOrderDetail.DeliveryStatus = orderStatus.FirstOrDefault(r => r.OrderStatusLinkId == orderDetail.OrderStatusLinkId).Description;

                    userOrderDetails = userOrderDetails.Concat(new UserOrderDetail[] { userOrderDetail }).ToArray();
                }
            }

            return userOrderDetails;
        }

        private string getPaymentGateway(string orderId)
        {
            string paymentGateway = "";

            if (orderId.IndexOf("RZ-") == 0)
            {
                paymentGateway = "RazorPay";
            }

            return paymentGateway;
        }

        private Address getShippingAddress(User user, IList<Address>  addresses, int shoppingAddressIndex)
        {
            Address address = new Address();

            if (shoppingAddressIndex == 0)
            {
                address.CountryId = user.CountryId;
                address.StateId = user.StateId;
                address.CityId = user.CityId;
                address.PinCode = user.PinCode;
                address.Addresss = user.Address1;
                address.FlagCode = user.FlagCode;

                return address;
            }

            AddressComparator comparator = new AddressComparator();

            List<Address> sortAddress = addresses.ToList();
            sortAddress.Sort(comparator);

            return sortAddress[shoppingAddressIndex-1];
        }
        private string getAddress(OrderHeader orderHeader, string country, string state, string city)
        {
            string address = "";

            address += (orderHeader.FirstName + " ");
            address += (orderHeader.LastName + " ");
            address += (orderHeader.Address + " ");
            address += ("Country: " + country + " ");
            address += ("State: " + state + " ");
            address += ("City: " + city + " ");
            address += ("Pincode: " + orderHeader.Pincode + " ");
            address += ("MobileNo: " + orderHeader.MobileNo);

            return address;
        }

        private string getAddressOnly(OrderHeader orderHeader, string country, string state, string city)
        {
            string address = "";

            address += (orderHeader.Address + " ");
            address += ("Country: " + country + " ");
            address += ("State: " + state + " ");
            address += ("City: " + city + " ");
            address += ("Pincode: " + orderHeader.Pincode + " ");
            address += ("MobileNo: " + orderHeader.MobileNo);

            return address;
        }

        private IList<string> GetOrdersNos(IEnumerable<OrderDetails> orderDetails)
        {
            IList<string> orders = new List<string>();

            foreach (var orderDetail in orderDetails)
            {
                bool index = orders.Contains(orderDetail.UserOrderId);

                if (!orders.Contains(orderDetail.UserOrderId))
                    orders.Add(orderDetail.UserOrderId);
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

        private bool CheckDelivery(string pincode, string productId)
        {
            /// Supplier
            IEnumerable<User> users = _unitOfWork.UserRepository.GetAll(r => (r.UserRoleId == 3) && (r.Phone1.ToString().Substring(0, 2).Equals(productId.Substring(0, 2))) &&
                                                                        (r.PinCode.ToString().Substring(4, 2).Equals(productId.Substring(2, 2))) &&
                                                                        (r.Phone1.ToString().Substring(8, 2).Equals(productId.Substring(4, 2))));

            User user = users.FirstOrDefault();

            if (user.PinCode == Convert.ToInt32(pincode) || user.DeliveryPincodes.Contains(pincode))
                return true;

            return false;
        }
    }

    class AddressComparator : IComparer<Address>
    {
        public int Compare(Address address1, Address address2)
        {
            if (address1.AddressId < address2.AddressId)
                return -1;
            if (address1.AddressId > address2.AddressId)
                return 1;
            if (address1.AddressId == address2.AddressId)
                return 0;

            return 1;
        }
    }

    class DateComparator : IComparer<UserOrderHistory>
    {
        public int Compare(UserOrderHistory order1, UserOrderHistory order2)
        {
            return Compare(order1.OrderDate, order2.OrderDate);
        }

        private int Compare(string datetime1, string datetime2)
        {
            string[] datetimes1 = datetime1.Split(' ');
            string[] date1 = datetimes1[0].Split('-');
            string[] time1 = datetimes1[1].Split(':');
            long day1 = long.Parse(date1[2] + date1[1] + date1[0]);
            long hour1 = long.Parse(time1[0] + time1[1] + time1[2]);

            string[] datetimes2 = datetime2.Split(' ');
            string[] date2 = datetimes2[0].Split('-');
            string[] time2 = datetimes2[1].Split(':');
            long day2 = long.Parse(date2[2] + date2[1] + date2[0]);
            long hour2 = long.Parse(time2[0] + time2[1] + time2[2]);

            if (day1 == day2 && hour1 == hour2)
                return 0;
            if (day1 < day2)
                return -1;
            if (day1 > day2)
                return 1;
            if (day1 == day2 && hour1 < hour2)
                return -1;
            if (day1 == day2 && hour1 > hour2)
                return 1;

            return 0;
        }
    }
}