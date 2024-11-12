///#define FILESYSTEM

using eCommerce.DataAccess.Repository.IRepository;
using eCommerce.Models.ControllerModels;
using eCommerce.Models.Models;
using eCommerce.Models.ViewModels;
using eCommerce.UserManagement.Utils;
using eCommerce.FileSystem;
using Microsoft.AspNetCore.Mvc;
using System.IO;

///alter table yourTableName AUTO_INCREMENT=1;
///truncate table yourTableName;
///

namespace eCommerce.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExistingProductController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        ///private string _folder = "D:\\Shared\\Web\\eCommerce\\eCommerce.Angular\\eCommerce\\src\\assets\\eCommerce-Images";
        private string _folder = eCommerce.FileSystem.FileSystem.folder;

        public ExistingProductController(ILogger<WeatherForecastController> logger,
                                    IUnitOfWork unitOfWork/*,
                                    HttpContextAccessor httpContextAccessor*/)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("GetProducts")]
        public List<ProductDetail> GetProducts([FromQuery] string userproductid)
        {
            List<ProductDetail> list = new List<ProductDetail>();

            if (ModelState.IsValid)
            {
                IEnumerable<Product> productList = _unitOfWork.ProductRepository.GetAll(r => !r.UserProductId.Contains(userproductid));

                foreach (var product in productList)
                {
                    ProductDetail productDetail = new ProductDetail();

                    productDetail.Title = product.Title;
                    productDetail.Description = product.Description;

                    IEnumerable<ProductVariables> productVariables = _unitOfWork.ProductVariablesRepository.GetAll(r => r.UserProductId.Contains(product.UserProductId));

                    int index = 1;
                    foreach (var productVariable in productVariables)
                    {
                        ProductVariable variable = new ProductVariable();

                        variable.Index = index++;

                        Size size = _unitOfWork.SizeRepository.GetFirstOrDefault(r => r.SizeLinkId == productVariable.SizeLinkId);
                        variable.SizeDetail.SizeLinkId = size.SizeLinkId;
                        variable.SizeDetail.Sizecode = size.SizeCode;
                        variable.SizeDetail.Description = size.Description;

                        Color color = _unitOfWork.ColorRepository.GetFirstOrDefault(r => r.ColorLinkId == productVariable.ColorLinkId);
                        variable.ColorDetail.ColorLinkId = color.ColorLinkId;
                        variable.ColorDetail.Red = color.Red;
                        variable.ColorDetail.Green = color.Green;
                        variable.ColorDetail.Blue = color.Blue;
                        variable.ColorDetail.Description = color.Description;

                        variable.Price = productVariable.Price;
                        variable.Discount = productVariable.Discount;
                        variable.Inventory = productVariable.Inventory;
                        foreach (var imageUrl in productVariable.ImageUrls.Split(";"))
                        {
                            variable.ImageUrls.Add(imageUrl);
                        }

                        productDetail.ProductVariables.Add(variable);
                    }

                    productDetail.UserProductId = product.UserProductId;
                    productDetail.CategoryLinkId = product.CategoryLinkId;
                    productDetail.SubCategoryLinkId = product.SubCategoryLinkId;

                    list.Add(productDetail);
                }
            }

            return list;
        }

        [HttpGet]
        [Route("GetProductIds")]
        public List<string> GetProductIds([FromQuery] string userproductid)
        {
            List<string> list = new List<string>();

            if (ModelState.IsValid)
            {
                IEnumerable<Product> products = _unitOfWork.ProductRepository.GetAll(r => !r.UserProductId.Contains(userproductid));
                foreach (var product in products)
                {
                    string productId = product.UserProductId.Remove(product.UserProductId.IndexOf("-"));

                    if (!list.Contains(productId))
                        list.Add(productId);
                }

                list.Remove(userproductid.Remove(userproductid.IndexOf("-")));
            }

            return list;
        }

        [HttpGet]
        [Route("GetSelectedProducts")]
        public List<ProductDetail> GetSelectedProducts([FromQuery] string userproductid, [FromQuery] string productId)
        {
            List<ProductDetail> list = new List<ProductDetail>();

            if (ModelState.IsValid)
            {
                IEnumerable<Product> productList = _unitOfWork.ProductRepository.GetAll(r => !r.UserProductId.Contains(userproductid));

                foreach (var product in productList)
                {
                    ProductDetail productDetail = new ProductDetail();

                    productDetail.Title = product.Title;
                    productDetail.Description = product.Description;

                    IEnumerable<ProductVariables> productVariables = _unitOfWork.ProductVariablesRepository.GetAll(r => r.UserProductId.Contains(product.UserProductId));

                    int index = 1;
                    foreach (var productVariable in productVariables)
                    {
                        ProductVariable variable = new ProductVariable();

                        variable.Index = index++;

                        Size size = _unitOfWork.SizeRepository.GetFirstOrDefault(r => r.SizeLinkId == productVariable.SizeLinkId);
                        variable.SizeDetail.SizeLinkId = size.SizeLinkId;
                        variable.SizeDetail.Sizecode = size.SizeCode;
                        variable.SizeDetail.Description = size.Description;

                        Color color = _unitOfWork.ColorRepository.GetFirstOrDefault(r => r.ColorLinkId == productVariable.ColorLinkId);
                        variable.ColorDetail.ColorLinkId = color.ColorLinkId;
                        variable.ColorDetail.Red = color.Red;
                        variable.ColorDetail.Green = color.Green;
                        variable.ColorDetail.Blue = color.Blue;
                        variable.ColorDetail.Description = color.Description;

                        variable.Price = productVariable.Price;
                        variable.Discount = productVariable.Discount;
                        variable.Inventory = productVariable.Inventory;
                        foreach (var imageUrl in productVariable.ImageUrls.Split(";"))
                        {
                            variable.ImageUrls.Add(imageUrl);
                        }

                        productDetail.ProductVariables.Add(variable);
                    }

                    productDetail.UserProductId = product.UserProductId;
                    productDetail.CategoryLinkId = product.CategoryLinkId;
                    productDetail.SubCategoryLinkId = product.SubCategoryLinkId;

                    if (productDetail.UserProductId.Contains(productId))
                        list.Add(productDetail);
                    else if (productId.Equals("*"))
                        list.Add(productDetail);
                }
            }

            return list;
        }

#if FILESYSTEM
        [HttpGet]
        [Route("CopyProduct")]
        public IActionResult CopyProduct([FromQuery] string mobileNo, [FromQuery] string fromUserProductId, [FromQuery] string toUserProductId)
        {
            string actionName = "CopyProduct";

            if (ModelState.IsValid)
            {
                copyProduct(mobileNo, fromUserProductId, toUserProductId);
            }

            return CreatedAtAction(actionName, new { id = "success" });
        }

        [HttpGet]
        [Route("CopyProducts")]
        public IActionResult CopyProducts([FromQuery] string mobileNo, [FromQuery] string toBeAddedProductIds, [FromQuery] string toUserProductId)
        {
            string actionName = "CopyProducts";

            if (ModelState.IsValid)
            {
                string[] toBeAddedProducts = toBeAddedProductIds.Split(',');
                foreach (var fromUserProductId in toBeAddedProducts)
                {
                    copyProduct(mobileNo, fromUserProductId, toUserProductId);
                }
            }

            return CreatedAtAction(actionName, new { id = "success" });
        }

        private void copyProduct(string mobileNo, string fromUserProductId, string toUserProductId)
        {
            Product fromProduct = _unitOfWork.ProductRepository.GetFirstOrDefault(r => r.UserProductId.Equals(fromUserProductId));

            Product toProduct = new Product();
            toProduct.UserProductId = generateProductIdNumber(toUserProductId);
            toProduct.Title = fromProduct.Title;
            toProduct.Description = fromProduct.Description;
            toProduct.CategoryLinkId = fromProduct.CategoryLinkId;
            toProduct.SubCategoryLinkId = fromProduct.SubCategoryLinkId;

            _unitOfWork.ProductRepository.Add(toProduct);

            IEnumerable<ProductVariables> fromProductVariables = _unitOfWork.ProductVariablesRepository.GetAll(r => r.UserProductId.Equals(fromUserProductId));
            foreach (var fromProductVariable in fromProductVariables)
            {
                ProductVariables toProductVariable = new ProductVariables();
                toProductVariable.UserProductId = toProduct.UserProductId;
                toProductVariable.SizeLinkId = fromProductVariable.SizeLinkId;
                toProductVariable.ColorLinkId = fromProductVariable.ColorLinkId;
                toProductVariable.Price = 0;
                toProductVariable.Discount = 0;
                toProductVariable.Inventory = 0;
                toProductVariable.ImageUrls = fromProductVariable.ImageUrls;

                _unitOfWork.ProductVariablesRepository.Add(toProductVariable);
            }

            SupplierProduct supplierProduct = new SupplierProduct();
            supplierProduct.MobileNo = Convert.ToInt64(mobileNo);
            supplierProduct.UserProductId = toProduct.UserProductId;

            _unitOfWork.SupplierProductRepository.Add(supplierProduct);

            _unitOfWork.Save();

            DirectoryInfo fromFolder = new DirectoryInfo(_folder + "\\" + fromUserProductId);
            DirectoryInfo toFolder = new DirectoryInfo(_folder + "\\" + toProduct.UserProductId);

            ProductUtils.DeleteFiles(_folder + "\\" + toProduct.UserProductId);
            ProductUtils.CopyFolder(fromFolder, toFolder);
        }

        string generateProductIdNumber(string userProductId)
        {
            string productIdNumber = userProductId;
            int productId = 1;
            IEnumerable<ProductId> products = _unitOfWork.ProductRepository.GetAll(r => r.UserProductId.Contains(userProductId)).Select(x => new ProductId
            {
                UserProductId = x.UserProductId
            });

            foreach (var product in products)
            {
                product.UserProductId = product.UserProductId.Substring(product.UserProductId.IndexOf("-") + 1);
                productId = Math.Max(Convert.ToInt32(product.UserProductId), productId);
            }

            if (products.Count() > 0)
                ++productId;

            return productIdNumber + productId.ToString();
        }

#endif

        //TODO :: Fix the problem of duplicacy

        /**
        string generateProductIdNumber(string userProductId)
        {
            string productIdNumber = userProductId;
            int productId = _unitOfWork.ProductRepository.GetAll(r => r.UserProductId.Contains(userProductId)).Count() + 1;

            return productIdNumber + productId.ToString();
        }
        
        void CopyFolder(DirectoryInfo source, DirectoryInfo target)
        {
            Directory.CreateDirectory(target.FullName);

            // Copy each file into the new directory.
            foreach (FileInfo fi in source.GetFiles())
            {
                fi.CopyTo(Path.Combine(target.FullName, fi.Name), true);
            }

            // Copy each subdirectory using recursion.
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir =
                    target.CreateSubdirectory(diSourceSubDir.Name);
                CopyFolder(diSourceSubDir, nextTargetSubDir);
            }
        }
        */
    }
}
