using FileSystem.Models;
using FileSystem.Repository.IRepository;
using FileSystem.FileSystem;
using Microsoft.AspNetCore.Mvc;
using FileSystem.ControllerModels;
using FileSystem.Utils;

namespace FileSystem.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExistingProduct : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        private string _folder = FileFolder.folder;

        public ExistingProduct(ILogger<WeatherForecastController> logger,
                                      IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

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
    }
}