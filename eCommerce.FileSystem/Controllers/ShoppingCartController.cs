using FileSystem.ControllerModels;
using FileSystem.FileSystem;
using FileSystem.Models;
using FileSystem.Repository.IRepository;
using FileSystem.Utils;
using Microsoft.AspNetCore.Mvc;

namespace FileSystem.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ShoppingCartController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private string _folder = FileFolder.folder;

        public ShoppingCartController(ILogger<WeatherForecastController> logger,
                                        IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

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
    }
}