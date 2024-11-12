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
    public class ProductsController : ControllerBase
    {
        // Duplicate
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
        private class ProductId
        {
            public string productId;
        }

        private readonly ILogger<ProductsController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private string _folder = FileFolder.folder;

        public ProductsController(ILogger<ProductsController> logger,
                                    IUnitOfWork unitOfWork/*,
                                    HttpContextAccessor httpContextAccessor*/)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("GetProducts")]
        public ProductList GetProducts([FromQuery] string category, [FromQuery] string subCategory, [FromQuery] string city,
                                        [FromQuery] string pincode, [FromQuery] string storename)
        {
            ProductList list = new ProductList();

            list.Products = new List<Products>();

            if (city.Equals("null")) city = "";
            if (pincode.Equals("null")) pincode = "0";
            if (storename.Equals("null")) storename = "";

            IEnumerable<long> categoryIds = _unitOfWork.CategoryRepository.GetAll(r => r.Name.Equals(category)).Select(x => (long)x.CategoryLinkId);
            IEnumerable<long> subCategoryIds = _unitOfWork.SubCategoryRepository.GetAll(r => r.Name.Equals(subCategory)).Select(x => (long)x.SubCategoryLinkId);
            IEnumerable<int> cityIds = _unitOfWork.CityRepository.GetAll(r => r.Name.Equals(city)).Select(x => x.CityLinkId);

            long categoryId = categoryIds.FirstOrDefault();
            long subCategoryId = subCategoryIds.FirstOrDefault();
            int cityId = cityIds.FirstOrDefault();

            List<ProductData> listProductData = GetProductIds(categoryId, subCategoryId, city, cityId, Int32.Parse(pincode), storename).ToList();
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

            IEnumerable<Products> products = _unitOfWork.ProductRepository.GetAll(r => (r.CategoryLinkId == categoryId && r.SubCategoryLinkId == subCategoryId &&
                                                                                    productIds.Contains(r.UserProductId.Substring(0, 6)))).Select(x => new Products
                                                                                    {
                                                                                        ProductId = x.UserProductId,
                                                                                        Title = x.Title,
                                                                                        City = cities[productIds.IndexOf(x.UserProductId.Substring(0, 6))],
                                                                                        Pincode = pincodes[productIds.IndexOf(x.UserProductId.Substring(0, 6))],
                                                                                        Store = suppliers[productIds.IndexOf(x.UserProductId.Substring(0, 6))],
                                                                                    }
                                                                                  );

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

            return list;
        }

        [HttpGet]
        [Route("GetSearchProducts")]
        public ProductList GetSearchProducts([FromQuery] string search)
        {
            ProductList list = new ProductList();
            list.Products = new List<Products>();

            string[] searches = search.Split(',');
            int count = searches.ToList().Count();

            IEnumerable<Product> products = new List<Product>();
            foreach (string s in searches)
            {
                products = products.Concat(_unitOfWork.ProductRepository.GetAll(r => (r.Title.ToLower().Contains(s.ToLower()) || r.Description.ToLower().Contains(s.ToLower()))));
                count = products.Count();
                products = products.Concat(GetSearchProductsForCategory(s));
                count = products.Count();
                products = products.Concat(GetSearchProductsForSubCategory(s));
                count = products.Count();
            }

            count = products.Count();

            IEnumerable<Product> productsUnique = new List<Product>();

            foreach (Product product in products)
            {
                if (!productsUnique.Contains(product))
                    productsUnique = productsUnique.Append(product);

                int countt = productsUnique.Count();
            }

            count = productsUnique.Count();

            List<ProductData> listProductData = GetProductIds().ToList();
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

            IEnumerable<Products> searchProducts = new List<Products>();

            foreach (Product product in productsUnique)
            {
                Products productss = new Products();
                productss.ProductId = product.UserProductId;
                productss.Title = product.Title;
                productss.City = cities[productIds.IndexOf(product.UserProductId.Substring(0, 6))];
                productss.Pincode = pincodes[productIds.IndexOf(product.UserProductId.Substring(0, 6))];
                productss.Store = suppliers[productIds.IndexOf(product.UserProductId.Substring(0, 6))];

                searchProducts = searchProducts.Append(productss);
            }

            count = searchProducts.Count();

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

            searchProducts.ToList().ForEach(x =>
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

            return list;
        }

        private IEnumerable<ProductData> GetProductIds()
        {
            IEnumerable<City> allCities = _unitOfWork.CityRepository.GetAll();
            IEnumerable<ProductData> listProductData = new List<ProductData>();

            listProductData = _unitOfWork.UserRepository.GetAll(r => r.UserRoleId == 3).Select(x => new ProductData
            {
                productId = ProductUtils.GetProductId(x.Phone1, x.PinCode),
                phoneNo = x.Phone1,
                pincode = x.PinCode,
                supplier = x.StoreName,
                city = GetCity(x.CityId, allCities),
            });

            return listProductData;
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

        private IEnumerable<Product> GetSearchProductsForCategory(string search)
        {
            IEnumerable<Product> products = new List<Product>();
            IEnumerable<Category> categories = _unitOfWork.CategoryRepository.GetAll(r => r.Name.ToLower().Equals(search.ToLower()));

            if (categories.Count() == 0)
                return products;

            Category category = categories.FirstOrDefault();

            IEnumerable<SubCategory> subCategories = _unitOfWork.SubCategoryRepository.GetAll(r => r.CategoryLinkId == category.CategoryLinkId);

            foreach (SubCategory subCategory in subCategories)
            {
                products = products.Concat(_unitOfWork.ProductRepository.GetAll(r => (r.CategoryLinkId == category.CategoryLinkId && r.SubCategoryLinkId == subCategory.SubCategoryLinkId)));
                int count = products.Count();
            }

            return products;
        }

        private IEnumerable<Product> GetSearchProductsForSubCategory(string search)
        {
            IEnumerable<Product> products = new List<Product>();
            IEnumerable<SubCategory> subCategories = _unitOfWork.SubCategoryRepository.GetAll(r => r.Name.ToLower().Equals(search.ToLower()));

            if (subCategories.Count() == 0)
                return products;

            SubCategory subCategory = subCategories.FirstOrDefault();

            products = _unitOfWork.ProductRepository.GetAll(r => (r.CategoryLinkId == subCategory.CategoryLinkId && r.SubCategoryLinkId == subCategory.SubCategoryLinkId));

            return products;
        }
    }
}