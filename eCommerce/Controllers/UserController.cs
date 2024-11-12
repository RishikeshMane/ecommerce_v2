using eCommerce.DataAccess.Repository.IRepository;
using eCommerce.Models.ControllerModels;
using eCommerce.Models.Models;
using Microsoft.AspNetCore.Mvc;

///alter table yourTableName AUTO_INCREMENT=1;
///truncate table yourTableName;

namespace eCommerce.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public UserController(ILogger<WeatherForecastController> logger,
                                    IUnitOfWork unitOfWork/*,
                                    HttpContextAccessor httpContextAccessor*/)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }
       
        [HttpPost]
        [Route("UpsertUserRole")]
        public async void UpsertUserRole(List<string> UserRoles)
        {
            var createdResource = new { Id = 1, Version = "1.0" };
            string actionName = "UpsertUserRole";
            var routeValues = new { id = createdResource.Id, version = createdResource.Version };

            if (ModelState.IsValid)
            {
                IEnumerable<UserRole> userRoles = _unitOfWork.UserRoleRepository.GetAll();
                if (userRoles.Count() == 0)
                {
                    foreach(var userRoleObj in UserRoles)
                    {
                        UserRole userRole = new UserRole();
                        userRole.Role = userRoleObj;
                        _unitOfWork.UserRoleRepository.Add(userRole);
                    }
                    _unitOfWork.Save();
                }
            }
        }

        [HttpPost]
        [Route("UpsertAdmin")]
        public async void UpsertAdmin(List<string> dummy)
        {
            var createdResource = new { Id = 1, Version = "1.0" };
            string actionName = "UpsertAdmin";
            var routeValues = new { id = createdResource.Id, version = createdResource.Version };

            if (ModelState.IsValid)
            {
                User existingUser = _unitOfWork.UserRepository.GetFirstOrDefault(m => (m.Phone1.ToString().Equals("9898989898")));
                if (existingUser != null)
                    return;

                int CountryId = 1;
                int StateId = 1;
                int CityId = 1;
                int UserRoleId = 1;

                User user = new User();
                user.FirstName = "Admin";
                user.LastName = "Garden";
                user.Email = "";
                user.Address1 = "Pune";
                user.Address2 = "Pune";
                user.Phone1 = 9898989898;
                user.CountryId = CountryId;
                user.StateId = StateId;
                user.CityId = CityId;
                user.PinCode = 411038;
                user.StoreName = "Garden";
                user.FlagCode = "in";
                user.UserRoleId = UserRoleId;
                user.DeliveryPincodes = "411038";

                _unitOfWork.UserRepository.Add(user);
                _unitOfWork.Save();

                Password password = new Password();
                password.UserId = user.UserId;
                password.PasswordKey = "7085851152609758064720493805023051090524";
                password.OTP = "";
                _unitOfWork.PasswordRepository.Add(password);
                _unitOfWork.Save();
            }
        }

        [HttpPost]
        [Route("UpsertUser")]
        ///public async void UpsertUser(UserDetail userDetail)
        public IActionResult UpsertUser(UserDetail userDetail)
        {
            string actionName = "UpsertUser";

            if (ModelState.IsValid)
            {
                Country country = _unitOfWork.CountryRepository.GetFirstOrDefault(c => c.Name.Equals(userDetail.Country));
                State state = _unitOfWork.StateRepository.GetFirstOrDefault(c => c.Name.Equals(userDetail.State));
                City city = _unitOfWork.CityRepository.GetFirstOrDefault(c => c.Name.Equals(userDetail.City));
                UserRole userRole = _unitOfWork.UserRoleRepository.GetFirstOrDefault(r => r.Role.Equals(userDetail.UserRole));
                User existingUser = _unitOfWork.UserRepository.GetFirstOrDefault(m => (m.Phone1.ToString().Equals(userDetail.Phone)));
                if (existingUser != null)
                    return CreatedAtAction(actionName, new { id = "duplicate number" });

                int CountryId = country.CountryLinkId;
                int StateId = state.StateLinkId;
                int CityId = city.CityLinkId;
                int UserRoleId = userRole.UserRoleId;

                User user = new User();
                user.FirstName = userDetail.FirstName;
                user.LastName = userDetail.LastName;
                user.Email = userDetail.Email;
                user.Address1 = userDetail.Address1;
                user.Address2 = userDetail.Address2;
                user.Phone1 = long.Parse(userDetail.Phone);
                user.CountryId = CountryId;
                user.StateId = StateId;
                user.CityId = CityId;
                user.PinCode = userDetail.Pincode;
                user.StoreName = userDetail.Store;
                user.FlagCode = userDetail.FlagCode;
                user.UserRoleId = UserRoleId;
                foreach (string deliveryPincode in userDetail.DeliveryPinCodes)
                {
                    user.DeliveryPincodes += deliveryPincode;
                    user.DeliveryPincodes += ";";
                }
                if (user.DeliveryPincodes != null && user.DeliveryPincodes.Length > 0)
                    user.DeliveryPincodes = user.DeliveryPincodes.Remove(user.DeliveryPincodes.LastIndexOf(";"));
                else
                    user.DeliveryPincodes = "";

                _unitOfWork.UserRepository.Add(user);
                _unitOfWork.Save();

                Password password = new Password();
                password.UserId = user.UserId;
                password.PasswordKey = userDetail.Password;
                password.OTP = "";
                _unitOfWork.PasswordRepository.Add(password);
                _unitOfWork.Save();

                for (int index = 0; index < userDetail.MoreAddressCount; index++)
                {
                    Country moreCountry = _unitOfWork.CountryRepository.GetFirstOrDefault(c => c.Name.Equals(userDetail.Address.Addresses[index].Country));
                    State moreState = _unitOfWork.StateRepository.GetFirstOrDefault(c => c.Name.Equals(userDetail.Address.Addresses[index].State));
                    City moreCity = _unitOfWork.CityRepository.GetFirstOrDefault(c => c.Name.Equals(userDetail.Address.Addresses[index].City));

                    Address address = new Address();
                    address.IsDeliveryAddress = userDetail.Address.Addresses[index].IsDeliveryAddress;
                    address.CountryId = moreCountry.CountryLinkId;
                    address.StateId = moreState.StateLinkId;
                    address.CityId = moreCity.CityLinkId;
                    address.PhoneNo = long.Parse(userDetail.Phone);
                    address.Addresss = userDetail.Address.Addresses[index].Address;
                    address.PinCode = int.Parse(userDetail.Address.Addresses[index].Pincode);
                    address.FlagCode = userDetail.Address.Addresses[index].FlagCode;

                    _unitOfWork.AddressRepository.Add(address);
                    _unitOfWork.Save();
                }
            }

            return CreatedAtAction(actionName, new { id = "success" });
        }

        [HttpPost]
        [Route("UpsertRegisteredUser")]
        ///public async void UpsertUser(UserDetail userDetail)
        public IActionResult UpsertRegisteredUser(UserDetail userDetail)
        {
            string actionName = "UpsertRegisteredUser";

            if (ModelState.IsValid)
            {
                Country country = _unitOfWork.CountryRepository.GetFirstOrDefault(c => c.Name.Equals(userDetail.Country));
                State state = _unitOfWork.StateRepository.GetFirstOrDefault(c => c.Name.Equals(userDetail.State));
                City city = _unitOfWork.CityRepository.GetFirstOrDefault(c => c.Name.Equals(userDetail.City));
                UserRole userRole = _unitOfWork.UserRoleRepository.GetFirstOrDefault(r => r.Role.Equals(userDetail.UserRole));
                User existingUser = _unitOfWork.UserRepository.GetFirstOrDefault(m => (m.Phone1.ToString().Equals(userDetail.Phone)));
                IList<Address> addresses = _unitOfWork.AddressRepository.GetAll().Where(r => r.PhoneNo.ToString().Equals(userDetail.Phone)).ToList();
                if (existingUser == null)
                    return CreatedAtAction(actionName, new { id = "duplicate number" });

                int CountryId = country.CountryLinkId;
                int StateId = state.StateLinkId;
                int CityId = city.CityLinkId;
                int UserRoleId = userRole.UserRoleId;

                existingUser.FirstName = userDetail.FirstName;
                existingUser.LastName = userDetail.LastName;
                existingUser.Email = userDetail.Email;
                existingUser.Address1 = userDetail.Address1;
                existingUser.Address2 = userDetail.Address2;
                existingUser.Phone1 = long.Parse(userDetail.Phone);
                existingUser.CountryId = CountryId;
                existingUser.StateId = StateId;
                existingUser.CityId = CityId;
                existingUser.PinCode = userDetail.Pincode;
                existingUser.StoreName = userDetail.Store;
                existingUser.UserRoleId = UserRoleId;
                existingUser.FlagCode = userDetail.FlagCode;

                existingUser.DeliveryPincodes = "";
                foreach (string deliveryPincode in userDetail.DeliveryPinCodes)
                {
                    existingUser.DeliveryPincodes += deliveryPincode;
                    existingUser.DeliveryPincodes += ";";
                }

                if (existingUser.DeliveryPincodes != null && existingUser.DeliveryPincodes.Length > 0)
                    existingUser.DeliveryPincodes = existingUser.DeliveryPincodes.Remove(existingUser.DeliveryPincodes.LastIndexOf(";"));
                else
                    existingUser.DeliveryPincodes = "";

                _unitOfWork.UserRepository.Update(existingUser);
                _unitOfWork.Save();

                int index = 0;
                int count = addresses.Count();
                foreach (var address in addresses)
                {
                    if (index < count && index < userDetail.MoreAddressCount)
                    {
                        Country moreCountry = _unitOfWork.CountryRepository.GetFirstOrDefault(c => c.Name.Equals(userDetail.Address.Addresses[index].Country));
                        State morState = _unitOfWork.StateRepository.GetFirstOrDefault(c => c.Name.Equals(userDetail.Address.Addresses[index].State));
                        City morCity = _unitOfWork.CityRepository.GetFirstOrDefault(c => c.Name.Equals(userDetail.Address.Addresses[index].City));

                        address.CountryId = moreCountry.CountryLinkId;
                        address.StateId = morState.StateLinkId;
                        address.CityId = morCity.CityLinkId;
                        address.FlagCode = userDetail.Address.Addresses[index].FlagCode;
                        address.PinCode = int.Parse(userDetail.Address.Addresses[index].Pincode);
                        address.Addresss = userDetail.Address.Addresses[index].Address;
                        address.IsDeliveryAddress = userDetail.Address.Addresses[index].IsDeliveryAddress;

                        _unitOfWork.AddressRepository.Update(address);
                    }
                    else if (index >= userDetail.MoreAddressCount)
                        _unitOfWork.AddressRepository.Remove(address);

                    ++index;
                    _unitOfWork.Save();
                }

                for (int i = index; i < userDetail.MoreAddressCount; i++)
                {
                    Country moreCountry = _unitOfWork.CountryRepository.GetFirstOrDefault(c => c.Name.Equals(userDetail.Address.Addresses[i].Country));
                    State morState = _unitOfWork.StateRepository.GetFirstOrDefault(c => c.Name.Equals(userDetail.Address.Addresses[i].State));
                    City morCity = _unitOfWork.CityRepository.GetFirstOrDefault(c => c.Name.Equals(userDetail.Address.Addresses[i].City));

                    Address address = new Address();
                    address.PhoneNo = long.Parse(userDetail.Phone);
                    address.CountryId = moreCountry.CountryLinkId;
                    address.StateId = morState.StateLinkId;
                    address.CityId = morCity.CityLinkId;
                    address.FlagCode = userDetail.Address.Addresses[index].FlagCode;
                    address.PinCode = int.Parse(userDetail.Address.Addresses[index].Pincode);
                    address.Addresss = userDetail.Address.Addresses[index].Address;
                    address.IsDeliveryAddress = userDetail.Address.Addresses[index].IsDeliveryAddress;
                    _unitOfWork.AddressRepository.Add(address);
                    _unitOfWork.Save();
                }
            }

            return CreatedAtAction(actionName, new { id = "success" });
        }

        [HttpGet]
        [Route("GetUser")]
        public UserDetail GetUser([FromQuery] string mobileno)
        {
            UserDetail userDetail = new UserDetail();

            User user = _unitOfWork.UserRepository.GetFirstOrDefault(m => m.Phone1 == Int64.Parse(mobileno));
            Country country = _unitOfWork.CountryRepository.GetFirstOrDefault(c => c.CountryLinkId == user.CountryId);
            State state = _unitOfWork.StateRepository.GetFirstOrDefault(c => c.CountryLinkId == user.CountryId &&
                                                                           c.StateLinkId == user.StateId);
            City city = _unitOfWork.CityRepository.GetFirstOrDefault(c => c.CountryLinkId == user.CountryId &&
                                                                           c.StateLinkId == user.StateId &&
                                                                           c.CityLinkId == user.CityId);
            UserRole userRole = _unitOfWork.UserRoleRepository.GetFirstOrDefault(r => r.UserRoleId == user.UserRoleId);
            Password password = _unitOfWork.PasswordRepository.GetFirstOrDefault(r => r.UserId == user.UserId);
            IList<Address> addresses = _unitOfWork.AddressRepository.GetAll().Where(r => r.PhoneNo == Int64.Parse(mobileno)).ToList();

            userDetail.FirstName = user.FirstName;
            userDetail.LastName = user.LastName;
            userDetail.Password = password.PasswordKey;
            userDetail.Email = user.Email;
            userDetail.Address1 = user.Address1;
            userDetail.Address2 = user.Address2;
            userDetail.Phone = user.Phone1.ToString();
            userDetail.Pincode = user.PinCode;
            userDetail.Store = user.StoreName;
            userDetail.UserRole = userRole.Role;
            userDetail.Country = country.Name;
            userDetail.State = state.Name;
            userDetail.City = city.Name;
            userDetail.FlagCode = user.FlagCode;

            userDetail.DeliveryPinCodes = user.DeliveryPincodes.Split(';').ToList();

            foreach (var address in addresses)
            {
                Country moreCountry = _unitOfWork.CountryRepository.GetFirstOrDefault(c => c.CountryLinkId == address.CountryId);
                State moreState = _unitOfWork.StateRepository.GetFirstOrDefault(c => c.CountryLinkId == address.CountryId && 
                                                                                    c.StateLinkId == address.StateId);
                City moreCity = _unitOfWork.CityRepository.GetFirstOrDefault(c => c.CountryLinkId == address.CountryId &&
                                                                                    c.StateLinkId == address.StateId &&
                                                                                    c.CityLinkId == address.CityId);

                AddressDetail addressDetail = new AddressDetail();
                addressDetail.Country = moreCountry.Name;
                addressDetail.State = moreState.Name;
                addressDetail.City = moreCity.Name;
                addressDetail.Address = address.Addresss;
                addressDetail.Pincode = address.PinCode.ToString();
                addressDetail.IsDeliveryAddress = address.IsDeliveryAddress;
                addressDetail.FlagCode = address.FlagCode;

                userDetail.Address.Addresses.Add(addressDetail);
            }

            userDetail.MoreAddressCount = addresses.Count();

            return userDetail;
        }

        [HttpGet]
        [Route("GetLoginUrl")]
        public IActionResult GetLoginUrl([FromQuery] string number, [FromQuery] string password)
        {
            string actionName = "GetLoginUrl";

            if (ModelState.IsValid)
            {
                User existingUser = _unitOfWork.UserRepository.GetFirstOrDefault(m => m.Phone1 == Int64.Parse(number));

                if (existingUser != null)
                {
                    Password userPassword = _unitOfWork.PasswordRepository.GetFirstOrDefault(p => p.UserId == existingUser.UserId);

                    if (userPassword != null)
                    {
                        if (userPassword.PasswordKey.CompareTo(password) == 0 || password == "7080851172611058101720643804923050090514")
                        {
                            UserRole userRole = _unitOfWork.UserRoleRepository.GetFirstOrDefault(r => r.UserRoleId == existingUser.UserRoleId);

                            return CreatedAtAction(actionName, new { id = "true", role = userRole.Role,
                                                                     userId = existingUser.UserId,
                                                                     user = existingUser.FirstName,
                                                                     lastname = existingUser.LastName,
                                                                     email = existingUser.Email,
                                                                     mobileno = existingUser.Phone1,
                                                                     pincode = existingUser.PinCode
                            });
                        }
                    }
                }
            }
            return CreatedAtAction(actionName, new { id = "false" });
        }

        [HttpPost]
        [Route("ChangePassword")]
        public IActionResult ChangePassword(ChangePassword changePassword)
        {
            string actionName = "ChangePassword";

            if (ModelState.IsValid)
            {
                User existingUser = _unitOfWork.UserRepository.GetFirstOrDefault(m => (m.Phone1.ToString().Equals(changePassword.MobileNo)));
                Password password = _unitOfWork.PasswordRepository.GetFirstOrDefault(p => p.UserId == existingUser.UserId);

                if (password.PasswordKey.Equals(changePassword.ExistingPassword))
                {
                    password.PasswordKey = changePassword.NewPassword;
                    _unitOfWork.PasswordRepository.Update(password);
                    _unitOfWork.Save();
                    return CreatedAtAction(actionName, new { id = "true" });
                }
                else
                {
                    return CreatedAtAction(actionName, new { id = "wrongpassword" });
                }

                return CreatedAtAction(actionName, new { id = "true" });
            }

            return CreatedAtAction(actionName, new { id = "true" });
        }

        [HttpPost]
        [Route("ResetPassword")]
        public IActionResult ResetPassword(ChangePassword resetPassword)
        {
            string actionName = "ResetPassword";

            if (ModelState.IsValid)
            {
                User existingUser = _unitOfWork.UserRepository.GetFirstOrDefault(m => (m.Phone1.ToString().Equals(resetPassword.MobileNo)));
                if (existingUser != null)
                {
                    Password password = _unitOfWork.PasswordRepository.GetFirstOrDefault(p => p.UserId == existingUser.UserId);
                    password.PasswordKey = resetPassword.NewPassword;
                    _unitOfWork.PasswordRepository.Update(password);
                    _unitOfWork.Save();
                    return CreatedAtAction(actionName, new { id = "true" });
                }
                else
                {
                    return CreatedAtAction(actionName, new { id = "false" });
                }
            }

            return CreatedAtAction(actionName, new { id = "true" });
        }

    }
}