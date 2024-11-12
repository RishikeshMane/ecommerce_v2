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
    public class ProductController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        ///private string _folder = "D:\\Shared\\Web\\eCommerce\\eCommerce.Angular\\eCommerce\\src\\assets\\eCommerce-Images";
        private string _folder = eCommerce.FileSystem.FileSystem.folder;
        private string _temp = "\\" + "Temp";

        public ProductController(ILogger<WeatherForecastController> logger,
                                    IUnitOfWork unitOfWork/*,
                                    HttpContextAccessor httpContextAccessor*/)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;

            _unitOfWork.UserRoleRepository.GetAll();
        }
       
        [HttpPost]
        [Route("UpsertSize")]
        public IActionResult UpsertSize(SizeList sizeList)
        {
            var createdResource = new { Id = 1, Version = "1.0" };
            string actionName = "UpsertSize";
            var routeValues = new { id = createdResource.Id, version = createdResource.Version };

            if (ModelState.IsValid)
            {
                IEnumerable<Size> sizes = _unitOfWork.SizeRepository.GetAll();
                if (sizes.Count() == 0)
                {
                    foreach(var sizeObj in sizeList.Size)
                    {
                        Size size = new Size();
                        size.SizeLinkId = sizeObj.SizeLinkId;
                        size.SizeCode = sizeObj.Sizecode;
                        size.Description = sizeObj.Description;
                        _unitOfWork.SizeRepository.Add(size);
                    }
                    _unitOfWork.Save();
                }
            }
            return CreatedAtAction(actionName, new { id = "success" });
        }

        [HttpPost]
        [Route("UpsertColor")]
        public IActionResult UpsertColor(ColorList colorList)
        {
            var createdResource = new { Id = 1, Version = "1.0" };
            string actionName = "UpsertColor";
            var routeValues = new { id = createdResource.Id, version = createdResource.Version };

            if (ModelState.IsValid)
            {
                IEnumerable<Color> colors = _unitOfWork.ColorRepository.GetAll();
                if (colors.Count() == 0)
                {
                    foreach (var colorObj in colorList.Color)
                    {
                        Color color = new Color();
                        color.ColorLinkId = colorObj.ColorLinkId;
                        color.Red = colorObj.Red;
                        color.Green = colorObj.Green;
                        color.Blue = colorObj.Blue;
                        color.Description = colorObj.Description;
                        _unitOfWork.ColorRepository.Add(color);
                    }
                    _unitOfWork.Save();
                }
            }

            return CreatedAtAction(actionName, new { id = "success" });
        }

        [HttpPost]
        [Route("UpdatePaymentGateway")]
        public IActionResult UpdatePaymentGateway(PaymentGatewayList paymentGateways)
        {
            var createdResource = new { Id = 1, Version = "1.0" };
            string actionName = "UpdatePaymentGateway";
            var routeValues = new { id = createdResource.Id, version = createdResource.Version };

            if (ModelState.IsValid)
            {
                IEnumerable<PaymentGateway> paymentGatewaysAll = _unitOfWork.PaymentGatewayRepository.GetAll();
                if (paymentGatewaysAll.Count() == 0)
                {
                    foreach (var paymentGatewayObj in paymentGateways.PaymentGatewayDetail)
                    {
                        PaymentGateway paymentGateway = new PaymentGateway();
                        paymentGateway.PaymentGatewayLinkId = paymentGatewayObj.PaymentGatewayLinkId.ToString();
                        paymentGateway.PaymentGatewayName = paymentGatewayObj.PaymentGatewayName;
                        paymentGateway.ActiveNow = paymentGatewayObj.ActiveNow;

                        _unitOfWork.PaymentGatewayRepository.Add(paymentGateway);
                    }
                    _unitOfWork.Save();
                }
            }

            return CreatedAtAction(actionName, new { id = "success" });
        }

        [HttpPost]
        [Route("UpdateSMSService")]
        public IActionResult UpdateSMSService(SMSServiceList smsServices)
        {
            var createdResource = new { Id = 1, Version = "1.0" };
            string actionName = "UpdateSMSService";
            var routeValues = new { id = createdResource.Id, version = createdResource.Version };

            if (ModelState.IsValid)
            {
                IEnumerable<SMSService> smsServiceAll = _unitOfWork.SMSServiceRepository.GetAll();
                if (smsServiceAll.Count() == 0)
                {
                    foreach (var smsServiceObj in smsServices.SMSServiceDetail)
                    {
                        SMSService smsService = new SMSService();
                        smsService.SMSServiceLinkId = smsServiceObj.SMSServiceLinkId.ToString();
                        smsService.SMSServiceName = smsServiceObj.SMSServiceName;
                        smsService.ActiveNow = smsServiceObj.ActiveNow;

                        _unitOfWork.SMSServiceRepository.Add(smsService);
                    }
                    _unitOfWork.Save();
                }
            }

            return CreatedAtAction(actionName, new { id = "success" });
        }

        [HttpPost]
        [Route("UpdateEMailService")]
        public IActionResult UpdateEMailService(EMailServiceList eMailServices)
        {
            var createdResource = new { Id = 1, Version = "1.0" };
            string actionName = "UpdateEMailService";
            var routeValues = new { id = createdResource.Id, version = createdResource.Version };

            if (ModelState.IsValid)
            {
                IEnumerable<EMailService> eMailServiceAll = _unitOfWork.EMailServiceRepository.GetAll();
                if (eMailServiceAll.Count() == 0)
                {
                    foreach (var eMailServiceObj in eMailServices.EMailServiceDetail)
                    {
                        EMailService eMailService = new EMailService();
                        eMailService.EMailServiceLinkId = eMailServiceObj.EMailServiceLinkId.ToString();
                        eMailService.EMailServiceName = eMailServiceObj.EMailServiceName;
                        eMailService.ActiveNow = eMailServiceObj.ActiveNow;

                        _unitOfWork.EMailServiceRepository.Add(eMailService);
                    }
                    _unitOfWork.Save();
                }
            }

            return CreatedAtAction(actionName, new { id = "success" });
        }

        [HttpPost]
        [Route("UpdateOrderStatus")]
        public IActionResult UpdateOrderStatus(OrderStatusList orders)
        {
            var createdResource = new { Id = 1, Version = "1.0" };
            string actionName = "UpdateOrderStatus";
            var routeValues = new { id = createdResource.Id, version = createdResource.Version };

            if (ModelState.IsValid)
            {
                IEnumerable<OrderStatus> orderStatusAll = _unitOfWork.OrderStatusRepository.GetAll();
                if (orderStatusAll.Count() == 0)
                {
                    foreach (var ordersObj in orders.OrderStatusDetail)
                    {
                        OrderStatus orderStatus = new OrderStatus();
                        orderStatus.OrderStatusLinkId = ordersObj.OrderStatusLinkId;
                        orderStatus.Description = ordersObj.OrderStatus;

                        _unitOfWork.OrderStatusRepository.Add(orderStatus);
                    }
                    _unitOfWork.Save();
                }
            }

            return CreatedAtAction(actionName, new { id = "success" });
        }

        [HttpGet]
        [Route("GetSizes")]
        public IEnumerable<Size> GetSizes()
        {
            return _unitOfWork.SizeRepository.GetAll();
        }

        [HttpGet]
        [Route("GetColors")]
        public IEnumerable<Color> GetColors()
        {
            return _unitOfWork.ColorRepository.GetAll();
        }

        [HttpGet]
        [Route("GetCategories")]
        public CategoryList GetCategories()
        {
            CategoryList categoryList = new CategoryList();

            if (ModelState.IsValid)
            {
                IEnumerable<Category> categories = _unitOfWork.CategoryRepository.GetAll();

                foreach (var categoryObj in categories)
                {
                    CategoryDetail categoryDetail = new CategoryDetail();
                    categoryDetail.Category = categoryObj.Name;
                    categoryDetail.CategoryLinkId = categoryObj.CategoryLinkId;

                    IEnumerable<SubCategory>  subCategories = _unitOfWork.SubCategoryRepository.GetAll(c => c.CategoryLinkId == categoryDetail.CategoryLinkId);

                    foreach (var subCategoryObj in subCategories)
                    {
                        categoryDetail.SubCategoryLinkIds.Add(subCategoryObj.SubCategoryLinkId);
                        categoryDetail.SubCategories.Add(subCategoryObj.Name);
                    }

                   categoryList.Category.Add(categoryDetail);
                }
            }

            return categoryList;
        }

        /**
        [HttpPost]
        [Consumes("multipart/form-data")]
        [Route("UpsertProduct")]
        public IActionResult UpsertProduct(/*[FromForm] ProductDetaill productDetail*/ /**)
        {
            IFormFileCollection files = this.Request.Form.Files;

            ICollection<string> keys = this.Request.Form.Keys;
            foreach (var key in keys)
            {
                string value = this.Request.Form[key];
                int i = 0;
            }

            return CreatedAtAction("UpsertProduct", new { id = "success" });
        }
        */

        [HttpGet]
        [Route("GetProducts")]
        public List<ProductDetail> GetProducts([FromQuery] string userproductid)
        {
            List<ProductDetail> list = new List<ProductDetail>();

            if (ModelState.IsValid)
            {
                IEnumerable<Product> productList = _unitOfWork.ProductRepository.GetAll(r => r.UserProductId.Contains(userproductid));

                foreach (var product in productList)
                {
                    ProductDetail productDetail = new ProductDetail();

                    productDetail.Title = product.Title;
                    productDetail.Description = product.Description;

                    IEnumerable<ProductVariables> productVariables = _unitOfWork.ProductVariablesRepository.GetAll(r => r.UserProductId == product.UserProductId);

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
        [Route("GetProductVariable")]
        public List<ProductVariable> GetProductVariable([FromQuery] string productid)
        {
            List<ProductVariable> list = new List<ProductVariable>();

            if (ModelState.IsValid)
            {
                IEnumerable<ProductVariables> productVariables = _unitOfWork.ProductVariablesRepository.GetAll(r => r.UserProductId.Equals(productid));
                IEnumerable<Size> sizes = _unitOfWork.SizeRepository.GetAll();
                IEnumerable<Color> colors = _unitOfWork.ColorRepository.GetAll();

                int index = 1;
                foreach (var variables in productVariables)
                {
                    ProductVariable variable = new ProductVariable();
                    variable.Index = index++;
                    variable.Price = variables.Price;
                    variable.Discount = variables.Discount;
                    variable.Inventory = variables.Inventory;

                    variable.SizeDetail.SizeLinkId = variables.SizeLinkId;
                    foreach(var size in sizes)
                    {
                        if (size.SizeLinkId == variables.SizeLinkId)
                        {
                            variable.SizeDetail.sizeId = size.SizeId;
                            variable.SizeDetail.Sizecode = size.SizeCode;
                            variable.SizeDetail.Description = size.Description;

                            break;
                        }
                    }

                    variable.ColorDetail.ColorLinkId = variables.ColorLinkId;
                    foreach (var color in colors)
                    {
                        if (color.ColorLinkId == variables.ColorLinkId)
                        {
                            variable.ColorDetail.ColorId = color.ColorId;
                            variable.ColorDetail.Red = color.Red;
                            variable.ColorDetail.Green = color.Green;
                            variable.ColorDetail.Blue = color.Blue;
                            variable.ColorDetail.Description = color.Description;

                            break;
                        }
                    }

                    foreach(string imageUrl in variables.ImageUrls.Split(";"))
                    {
                        if (!string.IsNullOrEmpty(imageUrl))
                            variable.ImageUrls.Add(imageUrl);
                    }

                    list.Add(variable);
                }
            }

            return list;
        }

#if FILESYSTEM
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
#endif
        /**
        void deleteFiles(string path)
        {
            if (Directory.Exists(path))
            {
                var dir = new DirectoryInfo(path);
                dir.Attributes = dir.Attributes & ~FileAttributes.ReadOnly;
                dir.Delete(true);
            }
        }
        */
        //TODO :: duplicacy

        /**
        string generateProductIdNumber(string userProductId)
        {
            string productIdNumber = userProductId;
            int productId = _unitOfWork.ProductRepository.GetAll(r => r.UserProductId.Contains(userProductId)).Count()+1;

            return productIdNumber + productId.ToString();
        }
        */
        /**
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

    class ProductId
    {
        public string UserProductId { get; set; }
    }
}