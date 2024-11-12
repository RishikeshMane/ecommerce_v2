using eCommerce.DataAccess.Repository.IRepository;
using eCommerce.Models.ControllerModels;
using eCommerce.Models.Models;
using eCommerce.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

///alter table yourTableName AUTO_INCREMENT=1;
///truncate table yourTableName;

namespace eCommerce.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        ///private string _folder = "D:\\Shared\\Web\\eCommerce\\eCommerce.Angular\\eCommerce\\src\\assets\\eCommerce-Images";
        ///private string _folder = "D:\\Shared\\Web\\FileSystem\\eCommerce-Images";
        ///private string _folder = "P:\\eCommerce-Images";
        private string _folder = eCommerce.FileSystem.FileSystem.folder;
        private string _home = "Home";
        private string _long = "Long";
        private string _deal = "Deal";
        private string _happy = "Happy";

        public HomeController(ILogger<WeatherForecastController> logger,
                                    IUnitOfWork unitOfWork/*,
                                    HttpContextAccessor httpContextAccessor*/)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;

            _unitOfWork.UserRoleRepository.GetAll();
        }

        [HttpGet]
        [Route("GetImageUrls")]
        public string[] GetImageUrls()
        {
            string[] urls = new string[] { };
            string[] lists = Directory.GetDirectories(_folder + "\\" + _home);

            foreach (var name in lists)
            {
                string[] productname = Directory.GetDirectories(name);

                foreach (var product in productname)
                {
                    string[] files = Directory.GetFiles(product);
                    string productName = product.Substring(product.LastIndexOf("\\")+1);
                    string subproduct = "";
                    foreach (var file in files)
                    {
                        string subproductName = file.Substring(file.LastIndexOf("\\") + 1);
                        subproduct += (subproductName + ";");
                    }

                    if (urls.Length < 9)
                        urls = urls.Append(productName + ":" + subproduct).ToArray();
                }
            }

            return urls;
        }

        [HttpGet]
        [Route("GetLongImageUrls")]
        public string[] GetLongImageUrls()
        {
            string[] urls = new string[] { };
            string[] lists = Directory.GetDirectories(_folder + "\\" + _home + "\\" + _long);

            foreach (var name in lists)
            {
                string[] productname = Directory.GetDirectories(name);

                foreach (var product in productname)
                {
                    string[] files = Directory.GetFiles(product);
                    string productName = product.Substring(product.LastIndexOf("\\") + 1);
                    string subproduct = "";
                    foreach (var file in files)
                    {
                        string subproductName = file.Substring(file.LastIndexOf("\\") + 1);
                        subproduct += (subproductName + ";");
                    }

                    if (urls.Length < 6)
                        urls = urls.Append(productName + ":" + subproduct).ToArray();
                }
            }

            return urls;
        }

        [HttpGet]
        [Route("GetDealImageUrls")]
        public string[] GetDealImageUrls()
        {
            string[] urls = new string[] { };
            string[] lists = Directory.GetDirectories(_folder + "\\" + _home + "\\" + _deal);

            string[] productname = Directory.GetFiles(lists[0]);

            string product = lists[0].Substring(lists[0].LastIndexOf("\\") + 1);
            string subproduct = productname[0].Substring(productname[0].LastIndexOf("\\") + 1);
            urls = urls.Append(product + ":" + subproduct).ToArray();

            return urls;
        }

        [HttpGet]
        [Route("GetHappyImageUrls")]
        public string[] GetHappyImageUrls()
        {
            string[] urls = new string[] { };
            string[] lists = Directory.GetFiles(_folder + "\\" + _home + "\\" + _happy);

            string product = lists[0].Substring(lists[0].LastIndexOf("\\") + 1);

            urls = urls.Append(product).ToArray();

            return urls;
        }

    }
}