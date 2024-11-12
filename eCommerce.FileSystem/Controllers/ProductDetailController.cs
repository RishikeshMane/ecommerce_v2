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

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private string _folder = FileFolder.folder;

        public ProductDetailController(ILogger<WeatherForecastController> logger,
                                        IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

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

        // GetCity
        private string GetCity(int cityId, IEnumerable<City> allCities)
        {
            City city = allCities.Where(x => x.CityLinkId == cityId).FirstOrDefault();
            return city.Name;
        }
    }
}