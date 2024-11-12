///#define FILESYSTEM

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
    public class ShoppingCartController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private string _folder = eCommerce.FileSystem.FileSystem.folder;

        public ShoppingCartController(IUnitOfWork unitOfWork/*,
                                    HttpContextAccessor httpContextAccessor*/)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        [Route("UpsertShoppingCart")]
        public IActionResult UpsertShoppingCart(ShoppingCart shoppingCart)
        {
            string actionName = "UpsertShoppingCart";

            if (ModelState.IsValid)
            {
                ShoppingCart cart = _unitOfWork.ShoppingCartRepository.GetFirstOrDefault(m => (m.MobileNo == shoppingCart.MobileNo) &&
                                                                                                (m.UserProductId.Equals(shoppingCart.UserProductId)) &&
                                                                                                (m.SizeLinkId == shoppingCart.SizeLinkId) &&
                                                                                                m.ColorLinkId == shoppingCart.ColorLinkId);

                if (cart == null)
                {
                    _unitOfWork.ShoppingCartRepository.Add(shoppingCart);
                }
                else
                {
                    cart.Quantity += shoppingCart.Quantity;
                    _unitOfWork.ShoppingCartRepository.Update(cart);
                }

                _unitOfWork.Save();
            }

            return CreatedAtAction(actionName, new { id = "success" });
        }

        [HttpGet]
        [Route("GetShoppingAddress")]
        public string[] GetShoppingAddress([FromQuery] string mobileNo)
        {
            string actionName = "GetShoppingAddress";

            string[] shoppingAddress = new string[] {""};

            if (ModelState.IsValid)
            {
                long MobileNo = long.Parse(mobileNo);

                User user = _unitOfWork.UserRepository.GetFirstOrDefault(m => (m.Phone1 == long.Parse(mobileNo)));
                IList<Address> addresses = _unitOfWork.AddressRepository.GetAll().Where(r => r.PhoneNo == long.Parse(mobileNo)).ToList();

                if (user == null)
                    return shoppingAddress;

                shoppingAddress[0] = user.FirstName + " ";
                shoppingAddress[0] += user.LastName + ". ";
                shoppingAddress[0] += user.Address1 + ". ";

                Country country = _unitOfWork.CountryRepository.GetFirstOrDefault(m => (m.CountryLinkId == user.CountryId));
                shoppingAddress[0] += "Country: ";
                shoppingAddress[0] += country.Name + ". ";

                State state = _unitOfWork.StateRepository.GetFirstOrDefault(m => (m.CountryLinkId == user.CountryId) && 
                                                                            (m.StateLinkId == user.StateId));
                shoppingAddress[0] += "State: ";
                shoppingAddress[0] += state.Name + ". ";

                City city = _unitOfWork.CityRepository.GetFirstOrDefault(m => (m.CountryLinkId == user.CountryId) && 
                                                                                 (m.StateLinkId == user.StateId) &&
                                                                                 (m.CityLinkId == user.CityId));
                shoppingAddress[0] += "City: ";
                shoppingAddress[0] += city.Name + ". ";

                shoppingAddress[0] += "Pincode: ";
                shoppingAddress[0] += user.PinCode + ". ";

                shoppingAddress[0] += "Mobile No: ";
                shoppingAddress[0] += user.Phone1.ToString();

                for (int index = 0; index < addresses.Count; index++)
                {
                    shoppingAddress = shoppingAddress.Concat(new string[] {""}).ToArray();

                    shoppingAddress[index+1] = user.FirstName + " ";
                    shoppingAddress[index+1] += user.LastName + ". ";
                    shoppingAddress[index+1] += addresses[index].Addresss + ". ";

                    Country moreCountry = _unitOfWork.CountryRepository.GetFirstOrDefault(m => (m.CountryLinkId == addresses[index].CountryId));
                    shoppingAddress[index+1] += "Country: ";
                    shoppingAddress[index+1] += moreCountry.Name + ". ";

                    State moreState = _unitOfWork.StateRepository.GetFirstOrDefault(m => (m.CountryLinkId == addresses[index].CountryId) && 
                                                                                            (m.StateLinkId == addresses[index].StateId));
                    shoppingAddress[index+1] += "State: ";
                    shoppingAddress[index+1] += moreState.Name + ". ";

                    City moreCity = _unitOfWork.CityRepository.GetFirstOrDefault(m => (m.CountryLinkId == addresses[index].CountryId) && 
                                                                                        (m.StateLinkId == addresses[index].StateId) && 
                                                                                        (m.CityLinkId == addresses[index].CityId));
                    shoppingAddress[index+1] += "City: ";
                    shoppingAddress[index+1] += moreCity.Name + ". ";

                    shoppingAddress[index+1] += "Pincode: ";
                    shoppingAddress[index+1] += addresses[index].PinCode + ". ";

                    shoppingAddress[index+1] += "Mobile No: ";
                    shoppingAddress[index+1] += user.Phone1.ToString();
                }
            }

            return shoppingAddress;
        }

        [HttpGet]
        [Route("GetShoppingAddressDetails")]
        public ShoppingAddress[] GetShoppingAddressDetails([FromQuery] string mobileNo)
        {
            string actionName = "GetShoppingAddressDetails";

            ShoppingAddress[] shoppingAddress = new ShoppingAddress[] { new ShoppingAddress(), new ShoppingAddress(), 
                                                                        new ShoppingAddress()};

            if (ModelState.IsValid)
            {
                long MobileNo = long.Parse(mobileNo);

                User user = _unitOfWork.UserRepository.GetFirstOrDefault(m => (m.Phone1 == long.Parse(mobileNo)));

                if (user == null)
                    return shoppingAddress;

                IList<Address> addresses = _unitOfWork.AddressRepository.GetAll().Where(r => r.PhoneNo == long.Parse(mobileNo)).ToList();

                for (int index = 0; index < addresses.Count; index++)
                {
                    shoppingAddress[index].Address = addresses[index].Addresss;

                    Country moreCountry = _unitOfWork.CountryRepository.GetFirstOrDefault(m => (m.CountryLinkId == addresses[index].CountryId));
                    shoppingAddress[index].SelectedCountryLinkId = moreCountry.CountryLinkId;
                    shoppingAddress[index].SelectedCountry = moreCountry.Name;

                    State moreState = _unitOfWork.StateRepository.GetFirstOrDefault(m => (m.CountryLinkId == addresses[index].CountryId) &&
                                                                                            (m.StateLinkId == addresses[index].StateId));
                    shoppingAddress[index].SelectedStateLinkId = moreState.StateLinkId;
                    shoppingAddress[index].SelectedState = moreState.Name;

                    City moreCity = _unitOfWork.CityRepository.GetFirstOrDefault(m => (m.CountryLinkId == addresses[index].CountryId) &&
                                                                                        (m.StateLinkId == addresses[index].StateId) &&
                                                                                        (m.CityLinkId == addresses[index].CityId));
                    shoppingAddress[index].SelectedCityLinkId = moreCity.CityLinkId;
                    shoppingAddress[index].SelectedCity = moreCity.Name;

                    shoppingAddress[index].Pincode = addresses[index].PinCode;
                    shoppingAddress[index].FlagCode = addresses[index].FlagCode;
                }
            }

            return shoppingAddress;
        }

        [HttpPost]
        [Route("UpdateAddress")]
        public string[] UpdateAddress(ShoppingAddress address, string mobileNo, string index)
        {
            string actionName = "UpdateAddress";

            if (ModelState.IsValid)
            {
                Country country = _unitOfWork.CountryRepository.GetFirstOrDefault(m => (m.Name == address.SelectedCountry));
                State state = _unitOfWork.StateRepository.GetFirstOrDefault(m => (m.Name == address.SelectedState));
                City city = _unitOfWork.CityRepository.GetFirstOrDefault(m => (m.Name == address.SelectedCity));

                IList<Address> addresses = _unitOfWork.AddressRepository.GetAll().Where(r => r.PhoneNo == long.Parse(mobileNo)).ToList();
                int addressIndex = int.Parse(index);
                int addressCount = addresses.Count;

                if (addressIndex <= addressCount)
                {
                    Address addresss = addresses[int.Parse(index) - 1];

                    addresss.CountryId = country.CountryLinkId;
                    addresss.StateId = state.StateLinkId;
                    addresss.CityId = city.CityLinkId;
                    addresss.PinCode = address.Pincode;
                    addresss.Addresss = address.Address;
                    addresss.FlagCode = address.FlagCode;

                    _unitOfWork.AddressRepository.Update(addresss);
                }
                else
                {
                    Address addresss = new Address();

                    addresss.CountryId = country.CountryLinkId;
                    addresss.StateId = state.StateLinkId;
                    addresss.CityId = city.CityLinkId;
                    addresss.PinCode = address.Pincode;
                    addresss.Addresss = address.Address;
                    addresss.PhoneNo = long.Parse(mobileNo);
                    addresss.FlagCode = address.FlagCode;

                    _unitOfWork.AddressRepository.Add(addresss);
                }

                _unitOfWork.Save();
            }

            return GetShoppingAddress(mobileNo);
        }

        [HttpPost]
        [Route("DeleteAddress")]
        public string[] DeleteAddress(string mobileNo)
        {
            string actionName = "DeleteAddress";

            if (ModelState.IsValid)
            {
                IList<Address> addresses = _unitOfWork.AddressRepository.GetAll().Where(r => r.PhoneNo == long.Parse(mobileNo)).ToList();

                if (addresses.Count > 0)
                {
                    Address addresss = addresses[addresses.Count-1];
                    _unitOfWork.AddressRepository.Remove(addresss);

                    _unitOfWork.Save();
                }
            }

            return GetShoppingAddress(mobileNo);
        }

#if FILESYSTEM
        [HttpGet]
        [Route("GetShoppingCartDetails")]
        public ShoppingCartDetails GetShoppingCartDetails([FromQuery] string mobileNo)
        {
            string actionName = "GetShoppingCartDetails";

            ShoppingCartDetails shoppingCartDetails = new ShoppingCartDetails();

            if (ModelState.IsValid)
            {
                long MobileNo = long.Parse(mobileNo);
                shoppingCartDetails.MobileNo = mobileNo;

                IEnumerable<ShoppingCart> carts = _unitOfWork.ShoppingCartRepository.GetAll(m => (m.MobileNo == MobileNo));

                foreach (var cart in carts)
                {
                    ShoppingProductDetail shoppingProductDetail = new ShoppingProductDetail();

                    shoppingProductDetail.SupplierMobileNo = cart.SupplierMobileNo.ToString();
                    shoppingProductDetail.UserProductId = cart.UserProductId;
                    shoppingProductDetail.SizeLinkId = cart.SizeLinkId;
                    shoppingProductDetail.ColorLinkId = cart.ColorLinkId;
                    shoppingProductDetail.Price = cart.Price;
                    shoppingProductDetail.Count = cart.Quantity;

                    IEnumerable<ProductVariables> productVariables = _unitOfWork.ProductVariablesRepository.GetAll(r => (r.UserProductId == cart.UserProductId));
                    int index = 1;
                    foreach (var productVariable in productVariables)
                    {
                        if ((productVariable.ColorLinkId == cart.ColorLinkId) && (productVariable.SizeLinkId == cart.SizeLinkId))
                        {
                            shoppingProductDetail.Index = index;
                            string[] images = productVariable.ImageUrls.Split(';');

                            if (images.Length > 0 && ProductUtils.FileExist(_folder, cart.UserProductId, index.ToString(), images[0]))
                            {
                                shoppingProductDetail.Image = images[0];
                            }

                            break;
                        }
                        index++;
                    }

                    Product product = _unitOfWork.ProductRepository.GetFirstOrDefault(r => (r.UserProductId == cart.UserProductId));
                    shoppingProductDetail.Title = product.Title;
                    shoppingProductDetail.Description = product.Description;

                    Size size = _unitOfWork.SizeRepository.GetFirstOrDefault(r => (r.SizeLinkId == cart.SizeLinkId));
                    shoppingProductDetail.SizeCode = size.SizeCode;

                    Color color = _unitOfWork.ColorRepository.GetFirstOrDefault(r => (r.ColorLinkId == cart.ColorLinkId));
                    shoppingProductDetail.ColorName = color.Description;

                    shoppingCartDetails.ProductDetail.Add(shoppingProductDetail);
                }
            }
            return shoppingCartDetails;
        }

        [HttpGet]
        [Route("DeleteProduct")]
        public ShoppingCartDetails DeleteProduct([FromQuery] string mobileNo, [FromQuery] string userProductId,
                                                    [FromQuery] string sizeLinkId, [FromQuery] string colorLinkId)
        {
            string actionName = "DeleteProduct";

            ShoppingCartDetails shoppingCartDetails = new ShoppingCartDetails();

            if (ModelState.IsValid)
            {
                ShoppingCart cart = _unitOfWork.ShoppingCartRepository.GetFirstOrDefault(m => (m.MobileNo == long.Parse(mobileNo)) &&
                                                                                                (m.UserProductId.Equals(userProductId)) &&
                                                                                                (m.SizeLinkId == int.Parse(sizeLinkId)) &&
                                                                                                (m.ColorLinkId == int.Parse(colorLinkId)));

                if (cart != null)
                {
                    _unitOfWork.ShoppingCartRepository.Remove(cart);
                    _unitOfWork.Save();
                }

                shoppingCartDetails = GetShoppingCartDetails(mobileNo);
            }

            return shoppingCartDetails;
        }
#endif
    }
}