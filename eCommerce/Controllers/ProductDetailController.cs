///#define FILESYSTEM

using eCommerce.DataAccess.Repository.IRepository;
using eCommerce.Models.ControllerModels;
using eCommerce.Models.Models;
using eCommerce.Models.ViewModels;
using eCommerce.UserManagement.Utils;
using Microsoft.AspNetCore.Mvc;
using System.IO;

///alter table yourTableName AUTO_INCREMENT=1;
///truncate table yourTableName;

namespace eCommerce.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductDetailController : ControllerBase
    {
        private class ProductData
        {
            public string productId;
            public string city;
            public long phoneNo;
            public int pincode;
            public string supplier;
        }

        private class ProductVariableData
        {
            public string productId;
            public string image;
            public int price;
            public int count;
        }

        private readonly ILogger<ProductsController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private string _folder = eCommerce.FileSystem.FileSystem.folder;

        public ProductDetailController(ILogger<ProductsController> logger,
                                    IUnitOfWork unitOfWork/*,
                                    HttpContextAccessor httpContextAccessor*/)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("GetProductDetail")]
        public ProductDetails GetProductDetail([FromQuery] string productId)
        {
            ProductDetails productDetail = new ProductDetails();

            Product product = _unitOfWork.ProductRepository.GetAll(r => r.UserProductId.Equals(productId)).FirstOrDefault();
            IEnumerable<ProductVariables> productVariables = _unitOfWork.ProductVariablesRepository.GetAll(r => r.UserProductId.Equals(productId));

            SupplierProduct supplierProduct = _unitOfWork.SupplierProductRepository.GetAll(r => r.UserProductId.Equals(productId)).FirstOrDefault();

            productDetail.Comments = GetProductComments(productId);

            IEnumerable<Size> sizes = _unitOfWork.SizeRepository.GetAll();
            IEnumerable<Color> colors = _unitOfWork.ColorRepository.GetAll();

            IEnumerable<Category> categories = _unitOfWork.CategoryRepository.GetAll();
            IEnumerable<SubCategory> subcategories = _unitOfWork.SubCategoryRepository.GetAll();

            productDetail.UserProductId = productId;
            productDetail.Title = product.Title;
            productDetail.Description = product.Description;
            productDetail.SupplierMobileno = supplierProduct.MobileNo;

            foreach (var productVariable in productVariables)
            {
                ProductVariable productvariable = new ProductVariable();

                productvariable.Price = productVariable.Price;
                productvariable.Discount = productVariable.Discount;
                productvariable.Inventory = productVariable.Inventory;
                productvariable.ImageUrls = productVariable.ImageUrls.Split(";").ToList();

                productvariable.SizeDetail.SizeLinkId = productVariable.SizeLinkId;
                productvariable.SizeDetail.Sizecode = sizes.Where(r => r.SizeLinkId == productVariable.SizeLinkId).Select(r => r.SizeCode).FirstOrDefault();
                productvariable.SizeDetail.Description = sizes.Where(r => r.SizeLinkId == productVariable.SizeLinkId).Select(r => r.Description).FirstOrDefault();

                productvariable.ColorDetail.ColorLinkId = productVariable.ColorLinkId;
                productvariable.ColorDetail.Red = colors.Where(r => r.ColorLinkId == productVariable.ColorLinkId).Select(r => r.Red).FirstOrDefault();
                productvariable.ColorDetail.Green = colors.Where(r => r.ColorLinkId == productVariable.ColorLinkId).Select(r => r.Green).FirstOrDefault();
                productvariable.ColorDetail.Blue = colors.Where(r => r.ColorLinkId == productVariable.ColorLinkId).Select(r => r.Blue).FirstOrDefault();
                productvariable.ColorDetail.Description = colors.Where(r => r.ColorLinkId == productVariable.ColorLinkId).Select(r => r.Description).FirstOrDefault();

                productDetail.ProductVariables.Add(productvariable);
            }

            productDetail.Category = categories.Where(r => r.CategoryLinkId == product.CategoryLinkId).Select(r => r.Name).FirstOrDefault();
            productDetail.SubCategory = subcategories.Where(r => (r.SubCategoryLinkId == product.SubCategoryLinkId && r.CategoryLinkId == product.CategoryLinkId)).Select(r => r.Name).FirstOrDefault();

            productDetail.CategoryLinkId = product.CategoryLinkId;
            productDetail.SubCategoryLinkId = product.SubCategoryLinkId;

            return productDetail;
        }

        [HttpPost]
        [Route("InsertProductComment")]
        public IActionResult InsertProductComment(ProductComments productComment)
        {
            string actionName = "InsertProductComment";

            if (ModelState.IsValid)
            {
                ProductComment comment = new ProductComment();
                comment.UserProductId = productComment.UserProductId;
                comment.UserId = Convert.ToInt64(productComment.UserId);
                comment.Rating = productComment.Rating;
                comment.Verified = 1;
                comment.Comment = productComment.Comment;

                _unitOfWork.ProductCommentRepository.Add(comment);
                _unitOfWork.Save();
            }

            return CreatedAtAction(actionName, new { id = "success" });
        }

        [HttpGet]
        [Route("CheckDelivery")]
        public IActionResult CheckDelivery([FromQuery] string pincode, [FromQuery] string productId)
        {
            string actionName = "CheckDelivery";

            if (pincode.Length != 6)
                return CreatedAtAction(actionName, new { id = "failure", productId = productId });

            if (ModelState.IsValid)
            {
                /// Supplier
                IEnumerable<User> users = _unitOfWork.UserRepository.GetAll(r => (r.UserRoleId == 3) && (r.Phone1.ToString().Substring(0,2).Equals(productId.Substring(0, 2))) &&
                                                                            (r.PinCode.ToString().Substring(4, 2).Equals(productId.Substring(2, 2))) &&
                                                                            (r.Phone1.ToString().Substring(8, 2).Equals(productId.Substring(4, 2))));

                User user = users.FirstOrDefault();

                if (user.PinCode == Convert.ToInt32(pincode) || user.DeliveryPincodes.Contains(pincode))
                    return CreatedAtAction(actionName, new { id = "success", productId = productId });
            }
            return CreatedAtAction(actionName, new { id = "failure", productId = productId });
        }

#if FILESYSTEM
        [HttpGet]
        [Route("GetGoesWellWith")]
        public GoesWellWith[] GetGoesWellWith([FromQuery] string productId, [FromQuery] string pincode,
                                                            [FromQuery] string storeName, [FromQuery] string city,
                                                            [FromQuery] string goesWell)
        {
            GoesWellWith[] goesWellWith = new GoesWellWith[] { };

            string[] lstGoesWellWith = goesWell.Split(',');

            foreach (string categorySubCategory in lstGoesWellWith)
            {
                GoesWellWith goesWellWell = GoesWellWith(pincode, storeName, city, categorySubCategory);
                if (goesWellWell.Pincode != 0 && goesWellWell.ProductId != null)
                    goesWellWith = goesWellWith.Append(goesWellWell).ToArray();
            }

            return goesWellWith;
        }

        GoesWellWith GoesWellWith(string pincode, string storeName,
                                    string city, string categorySubCategory)
        {
            GoesWellWith goesWellWith = new GoesWellWith();
            ProductList list = new ProductList();
            list.Products = new List<Products>();

            string categoryLinkid = categorySubCategory.Split("-")[0];
            string subCategoryLinkid = categorySubCategory.Split("-")[1];

            IEnumerable<string> category = _unitOfWork.CategoryRepository.GetAll(r => r.CategoryLinkId.Equals(Int32.Parse(categoryLinkid))).Select(x => x.Name);
            IEnumerable<string> subCategory = _unitOfWork.SubCategoryRepository.GetAll(r => r.SubCategoryLinkId.Equals(Int32.Parse(subCategoryLinkid)) && r.CategoryLinkId.Equals(Int32.Parse(categoryLinkid))).Select(x => x.Name);

            goesWellWith.Category = category.FirstOrDefault();
            goesWellWith.SubCategory = subCategory.FirstOrDefault();

            ///ProductsController.GetProducts();
            IEnumerable<int> cityIds = _unitOfWork.CityRepository.GetAll(r => r.Name.Equals(city)).Select(x => x.CityLinkId);
            int cityId = cityIds.FirstOrDefault();

            List<ProductData> listProductData = GetProductIds(Int32.Parse(categoryLinkid), Int32.Parse(subCategoryLinkid),
                                                                city, cityId, Int32.Parse(pincode), storeName).ToList();
            /// Scan All the pincodes
            if (listProductData.Count == 0)
            {
                listProductData = GetProductIds(Int32.Parse(categoryLinkid), Int32.Parse(subCategoryLinkid),
                                                                city, cityId, Int32.Parse(pincode), "").ToList();
                if (listProductData.Count == 0)
                {
                    int pinCode = Int32.Parse(pincode);

                    for (int indexAddPinCode = pinCode, indexMinusPinCode = pinCode; indexAddPinCode < pinCode + 20; indexAddPinCode++, indexMinusPinCode--)
                    {
                        listProductData = GetProductIds(Int32.Parse(categoryLinkid), Int32.Parse(subCategoryLinkid),
                                                                city, cityId, indexAddPinCode, "").ToList();
                        if (listProductData.Count == 0)
                        {
                            listProductData = GetProductIds(Int32.Parse(categoryLinkid), Int32.Parse(subCategoryLinkid),
                                                                    city, cityId, indexMinusPinCode, "").ToList();
                        }

                        if (listProductData.Count > 0)
                            break;
                    }
                }
            }

            if (listProductData.Count > 0)
            {
                List<string> productIds = new List<string>();
                List<int> pincodes = new List<int>();
                List<string> suppliers = new List<string>();
                List<string> cities = new List<string>();
                listProductData.ForEach(x =>
                {
                    productIds.Add(x.productId);
                    pincodes.Add(x.pincode);
                    suppliers.Add(x.supplier);
                    cities.Add(x.city);
                }
                );

                IEnumerable<Products> products = _unitOfWork.ProductRepository.GetAll(r => (r.CategoryLinkId == Int32.Parse(categoryLinkid) && r.SubCategoryLinkId == Int32.Parse(subCategoryLinkid) &&
                                                                                        productIds.Contains(r.UserProductId.Substring(0, 6)))).Select(x => new Products
                                                                                        {
                                                                                            ProductId = x.UserProductId,
                                                                                            Title = x.Title,
                                                                                            City = cities[productIds.IndexOf(x.UserProductId.Substring(0, 6))],
                                                                                            Pincode = pincodes[productIds.IndexOf(x.UserProductId.Substring(0, 6))],
                                                                                            Store = suppliers[productIds.IndexOf(x.UserProductId.Substring(0, 6))],
                                                                                        }
                                                                                      );

                IList<Products> lst = products.ToList();

                IEnumerable<ProductVariableData> productVariables = _unitOfWork.ProductVariablesRepository.GetAll(r => productIds.Contains(r.UserProductId.Substring(0, 6))).Select(x => new ProductVariableData
                {
                    productId = x.UserProductId,
                    image = x.ImageUrls,
                    price = x.Price,
                    count = x.Inventory,
                }
                );

                List<string> productId = new List<string>();
                List<string> images = new List<string>();
                List<int> prices = new List<int>();
                List<int> counts = new List<int>();
                productVariables.ToList().ForEach(x =>
                {
                    productId.Add(x.productId);
                    images.Add(x.image);
                    prices.Add(x.price);
                    counts.Add(x.count);
                }
                );

                products.ToList().ForEach(x =>
                {
                    Products product = new Products();

                    product.ProductId = x.ProductId;
                    product.Title = x.Title;
                    product.City = x.City;
                    product.Pincode = x.Pincode;
                    product.Store = x.Store;
                    if (images.Count > 0)
                        product.Image = images[productId.IndexOf(x.ProductId)].Split(';')[0];

                    product.Price = prices[productId.IndexOf(x.ProductId)];
                    product.Count = counts[productId.IndexOf(x.ProductId)];

                    if (product.Image.Length > 0 && images.Count > 0 && ProductUtils.FileExist(_folder, x.ProductId, product.Image))
                        list.Products.Add(product);
                });

                if (list.Products.Count > 0)
                {
                    goesWellWith.ProductId = list.Products[0].ProductId;
                    goesWellWith.Pincode = list.Products[0].Pincode;
                    goesWellWith.Store = list.Products[0].Store;
                    goesWellWith.City = list.Products[0].City;
                    goesWellWith.Image = list.Products[0].Image;
                    goesWellWith.Price = list.Products[0].Price;
                }

            }

            return goesWellWith;
        }

        /// Duplicate
        private IEnumerable<ProductData> GetProductIds(long categoryId, long subCategoryId, string cityName, int cityId, int pincode, string storename)
        {
            IEnumerable<City> allCities = _unitOfWork.CityRepository.GetAll();
            IEnumerable<ProductData> listProductData = new List<ProductData>();

            if (cityId == 0)
            {
                listProductData = _unitOfWork.UserRepository.GetAll(r => r.UserRoleId == 3).Select(x => new ProductData
                {
                    productId = ProductUtils.GetProductId(x.Phone1, x.PinCode),
                    phoneNo = x.Phone1,
                    pincode = x.PinCode,
                    supplier = x.StoreName,
                    city = GetCity(x.CityId, allCities),
                });
            }
            else if (cityId != 0 && pincode == 0)
            {
                listProductData = _unitOfWork.UserRepository.GetAll(r => r.UserRoleId == 3 && r.CityId == cityId).Select(x => new ProductData
                {
                    productId = ProductUtils.GetProductId(x.Phone1, x.PinCode),
                    phoneNo = x.Phone1,
                    pincode = x.PinCode,
                    supplier = x.StoreName,
                    city = cityName,
                });
            }
            else if (cityId != 0 && pincode != 0 && storename.Length == 0)
            {
                listProductData = _unitOfWork.UserRepository.GetAll(r => r.UserRoleId == 3 && r.CityId == cityId && r.PinCode == pincode).Select(x => new ProductData
                {
                    productId = ProductUtils.GetProductId(x.Phone1, x.PinCode),
                    phoneNo = x.Phone1,
                    pincode = x.PinCode,
                    supplier = x.StoreName,
                    city = cityName,
                });
            }
            else if (cityId != 0 && pincode != 0 && storename.Length > 0)
            {
                listProductData = _unitOfWork.UserRepository.GetAll(r => r.UserRoleId == 3 && r.CityId == cityId && r.PinCode == pincode && r.StoreName.Equals(storename)).Select(x => new ProductData
                {
                    productId = ProductUtils.GetProductId(x.Phone1, x.PinCode),
                    phoneNo = x.Phone1,
                    pincode = x.PinCode,
                    supplier = x.StoreName,
                    city = cityName,
                });
            }

            return listProductData;
        }
#endif

        // GetCity
        private string GetCity(int cityId, IEnumerable<City> allCities)
        {
            City city = allCities.Where(x => x.CityLinkId == cityId).FirstOrDefault();
            return city.Name;
        }

        private List<ProductComments> GetProductComments(string productId)
        {
            List<ProductComments> comments = new List<ProductComments>();

            IEnumerable<ProductComment> productComments = _unitOfWork.ProductCommentRepository.GetAll(r => (r.UserProductId.Equals(productId) && r.Verified == 1));
            IEnumerable<User> users = _unitOfWork.UserRepository.GetAll();
            foreach(var productComment in productComments)
            {
                ProductComments comment = new ProductComments();
                comment.UserProductId = productComment.UserProductId;
                comment.Rating = productComment.Rating;

                if (productComment.UserId == 0)
                {
                    comment.FirstName = "Anonymous";
                }
                else
                {
                    IEnumerable<User> user = _unitOfWork.UserRepository.GetAll(r => r.UserId == productComment.UserId);
                    User userName = user.FirstOrDefault();

                    IEnumerable<City> cities = _unitOfWork.CityRepository.GetAll(r => r.CityLinkId == userName.CityId);
                    City city = cities.FirstOrDefault();

                    IEnumerable<State> states = _unitOfWork.StateRepository.GetAll(r => r.StateLinkId == userName.StateId);
                    State state = states.FirstOrDefault();

                    comment.FirstName = userName.FirstName;
                    comment.LastName = userName.LastName;
                    comment.City = city.Name;
                    comment.State = state.Name;
                }

                comment.Comment = productComment.Comment;

                DateTime dateTime = productComment.CreatedDateTime.Date;
                comment.Date = ProductUtils.GetMonth(dateTime.Month) + "-";
                comment.Date += dateTime.Day.ToString() + "-";
                comment.Date += dateTime.Year.ToString();
                comments.Add(comment);
            }

            return comments;
        }
    }
}