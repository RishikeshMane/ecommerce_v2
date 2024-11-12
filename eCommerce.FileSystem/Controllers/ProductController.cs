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
    public class ProductController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private string _temp = "\\" + "Temp";

        private string _folder = FileFolder.folder;

        public ProductController(ILogger<WeatherForecastController> logger,
                                      IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        [Route("UpsertProduct")]
        public IActionResult UpsertProduct(ProductDetail productDetail)
        {
            System.Threading.Thread.Sleep(2000);
            var createdResource = new { Id = 1, Version = "1.0" };
            string actionName = "UpsertProduct";
            var routeValues = new { id = createdResource.Id, version = createdResource.Version };

            if (productDetail.addUpdate.Equals("Add"))
            {
                AddProduct(productDetail);
            }
            else if (productDetail.addUpdate.Equals("Update"))
            {
                UpdateProduct(productDetail);
            }

            return CreatedAtAction(actionName, new { id = "success" });
        }

        [HttpPost]
        [Route("UploadImage")]
        public IActionResult UploadImage()
        {
            IFormFileCollection files = this.Request.Form.Files;
            string productid = this.Request.Form["ProductId"];
            string mobileno = this.Request.Form["MobileNo"];
            string folder = _folder + "\\" + "Temp" + "\\" + mobileno + "\\" + this.Request.Form["ProductId"] + "\\" + this.Request.Form["Index"] + "\\";

            if (!productid.Contains("Add"))
            {
                folder = _folder + _temp + "\\" + this.Request.Form["ProductId"] + "\\" + this.Request.Form["Index"] + "\\";
            }

            string image = this.Request.Form["ImageName"];

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            foreach (var file in files)
            {
                HttpContext context = this.Request.HttpContext;
                HttpRequest request = context.Request;

                using (var fileStreams = new FileStream(folder + image, FileMode.Create))
                {
                    file.CopyTo(fileStreams);
                }
            }

            return CreatedAtAction("UploadImage", new { id = "success" });
        }

        [HttpDelete]
        [Route("DeleteProduct")]
        public IActionResult DeleteProduct(string userproductid)
        {
            var createdResource = new { Id = 1, Version = "1.0" };
            string actionName = "DeleteProduct";
            var routeValues = new { id = createdResource.Id, version = createdResource.Version };

            if (ModelState.IsValid)
            {
                SupplierProduct supplierproduct = _unitOfWork.SupplierProductRepository.GetFirstOrDefault(r => r.UserProductId.Equals(userproductid));
                _unitOfWork.SupplierProductRepository.Remove(supplierproduct);

                IEnumerable<ProductVariables> productVariables = _unitOfWork.ProductVariablesRepository.GetAll(r => r.UserProductId.Equals(userproductid));
                _unitOfWork.ProductVariablesRepository.RemoveRange(productVariables);

                Product product = _unitOfWork.ProductRepository.GetFirstOrDefault(r => r.UserProductId.Equals(userproductid));
                _unitOfWork.ProductRepository.Remove(product);

                _unitOfWork.Save();

                ProductUtils.DeleteFiles(_folder + "\\" + userproductid);
            }

            return CreatedAtAction(actionName, new { id = "success" });
        }

        [HttpPost]
        [Route("DeleteFiles")]
        public IActionResult DeleteFiles(VariableIndex variableIndex, string mobileno)
        {
            var createdResource = new { Id = 1, Version = "1.0" };
            string actionName = "DeleteFiles";
            var routeValues = new { id = createdResource.Id, version = createdResource.Version };

            if (ModelState.IsValid)
            {
                string imageRoot = "Temp" + "\\" + mobileno + "\\" + "Add";
                if (!variableIndex.ProductId.Contains("Add"))
                {
                    imageRoot = "Temp" + "\\" + variableIndex.ProductId;
                }

                if (variableIndex.ImagesUrls.Count() > 0)
                {
                    foreach (var image in variableIndex.ImagesUrls)
                    {
                        string imageName = GetImageName(image);
                        string file = _folder + "\\" + imageRoot + "\\" + variableIndex.Index + "\\" + imageName;
                        if (System.IO.File.Exists(file))
                        {
                            System.IO.File.Delete(file);
                        }
                    }
                }
                else
                {
                    string folder = _folder + "\\" + imageRoot + "\\" + variableIndex.Index;
                    ProductUtils.DeleteFiles(folder);
                }
            }

            return CreatedAtAction(actionName, new { id = "success" });
        }

        [HttpGet]
        [Route("DeleteJunkFiles")]
        public IActionResult DeleteJunkFiles(string mobileno, string productid, string addUpdate)
        {
            var createdResource = new { Id = 1, Version = "1.0" };
            string actionName = "DeleteJunkFiles";
            var routeValues = new { id = createdResource.Id, version = createdResource.Version };

            if (ModelState.IsValid)
            {
                ProductUtils.DeleteFiles(_folder + "\\" + "Temp" + "\\" + mobileno);

                if (!addUpdate.Equals("Add"))
                    ProductUtils.DeleteFiles(_folder + "\\" + "Temp" + "\\" + productid);
            }

            return CreatedAtAction(actionName, new { id = "success" });
        }

        [HttpPost]
        [Route("CopyFiles")]
        public IActionResult CopyFiles(VariableIndex variableIndex)
        {
            var createdResource = new { Id = 1, Version = "1.0" };
            string actionName = "CopyFiles";
            var routeValues = new { id = createdResource.Id, version = createdResource.Version };

            if (ModelState.IsValid)
            {
                if (Directory.Exists(_folder + "\\" + variableIndex.ProductId))
                {
                    string sourceFolder = _folder + "\\" + variableIndex.ProductId;
                    string destinationFolder = _folder + _temp + "\\" + variableIndex.ProductId;
                    DirectoryInfo dirSource = new DirectoryInfo(sourceFolder);
                    DirectoryInfo dirTarget = new DirectoryInfo(destinationFolder);
                    ProductUtils.DeleteFiles(destinationFolder);
                    ProductUtils.CopyFolder(dirSource, dirTarget);
                }
            }

            return CreatedAtAction(actionName, new { id = "success" });
        }

        private string GetImageName(string fileName)
        {
            return fileName.Substring(fileName.LastIndexOf('~') + 1);
        }

        private void AddProduct(ProductDetail productDetail)
        {
            if (ModelState.IsValid)
            {
                Product product = new Product();
                product.UserProductId = generateProductIdNumber(productDetail.UserProductId);
                product.Title = productDetail.Title;
                product.Description = productDetail.Description;
                product.CategoryLinkId = productDetail.CategoryLinkId;
                product.SubCategoryLinkId = productDetail.SubCategoryLinkId;

                _unitOfWork.ProductRepository.AddAsync(product);

                int destinationIndex = 1;

                foreach (var variable in productDetail.ProductVariables)
                {
                    ProductVariables productVariable = new ProductVariables();
                    productVariable.UserProductId = product.UserProductId;
                    productVariable.SizeLinkId = variable.SizeDetail.SizeLinkId;
                    productVariable.ColorLinkId = variable.ColorDetail.ColorLinkId;
                    productVariable.Price = variable.Price;
                    productVariable.Discount = variable.Discount;
                    productVariable.Inventory = variable.Inventory;
                    productVariable.ImageUrls = "";

                    foreach (var imageUrl in variable.ImageUrls)
                    {
                        productVariable.ImageUrls += imageUrl;
                        productVariable.ImageUrls += ";";

                        copyFiles(productDetail.UserMobileNo.ToString(), productVariable.UserProductId, variable.Index, destinationIndex, imageUrl, true);
                    }
                    ++destinationIndex;
                    _unitOfWork.ProductVariablesRepository.Add(productVariable);
                }

                ProductUtils.DeleteFiles(_folder + "\\" + "Temp" + "\\" + productDetail.UserMobileNo);

                SupplierProduct supplierProduct = new SupplierProduct();
                supplierProduct.MobileNo = productDetail.UserMobileNo;
                supplierProduct.UserProductId = product.UserProductId;

                _unitOfWork.SupplierProductRepository.Add(supplierProduct);
                _unitOfWork.Save();
            }
        }

        private void UpdateProduct(ProductDetail productDetail)
        {
            if (ModelState.IsValid)
            {
                Product product = _unitOfWork.ProductRepository.GetFirstOrDefault(r => r.UserProductId.Equals(productDetail.UserProductId));

                product.Title = productDetail.Title;
                product.Description = productDetail.Description;
                product.CategoryLinkId = productDetail.CategoryLinkId;
                product.SubCategoryLinkId = productDetail.SubCategoryLinkId;

                _unitOfWork.ProductRepository.Update(product);

                IEnumerable<ProductVariables> productVariables = _unitOfWork.ProductVariablesRepository.GetAll(r => r.UserProductId.Equals(productDetail.UserProductId));
                _unitOfWork.ProductVariablesRepository.RemoveRange(productVariables);
                _unitOfWork.Save();

                string destinationFolder = _folder + "\\" + product.UserProductId;
                ProductUtils.DeleteFiles(destinationFolder);
                int destinationIndex = 1;

                foreach (var variable in productDetail.ProductVariables)
                {
                    ProductVariables productVariable = new ProductVariables();
                    productVariable.UserProductId = product.UserProductId;
                    productVariable.SizeLinkId = variable.SizeDetail.SizeLinkId;
                    productVariable.ColorLinkId = variable.ColorDetail.ColorLinkId;
                    productVariable.Price = variable.Price;
                    productVariable.Discount = variable.Discount;
                    productVariable.Inventory = variable.Inventory;
                    productVariable.ImageUrls = "";

                    foreach (var imageUrl in variable.ImageUrls)
                    {
                        productVariable.ImageUrls += imageUrl;
                        productVariable.ImageUrls += ";";

                        copyFiles(productDetail.UserMobileNo.ToString(), productVariable.UserProductId, variable.Index, destinationIndex, imageUrl, false);
                    }
                    ++destinationIndex;
                    _unitOfWork.ProductVariablesRepository.Add(productVariable);
                }

                ProductUtils.DeleteFiles(_folder + _temp + "\\" + product.UserProductId);
                _unitOfWork.Save();
            }
        }

        void copyFiles(string mobileno, string productid, int sourceIndex, int destinationIndex, string image, bool addUpdate)
        {
            string fromImage = _folder + "\\" + "Temp" + "\\" + mobileno + "\\" + "Add" + "\\" + sourceIndex.ToString() + "\\" + image;
            if (!addUpdate)
                fromImage = _folder + "\\" + "Temp" + "\\" + productid + "\\" + sourceIndex.ToString() + "\\" + image;

            string toImage = _folder + "\\" + productid + "\\" + destinationIndex.ToString() + "\\" + image;

            if (!Directory.Exists(_folder + "\\" + productid + "\\" + destinationIndex.ToString()))
                Directory.CreateDirectory(_folder + "\\" + productid + "\\" + destinationIndex.ToString());

            if (System.IO.File.Exists(fromImage))
                System.IO.File.Copy(fromImage, toImage);
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

    class ProductId
    {
        public string UserProductId { get; set; }
    }
}