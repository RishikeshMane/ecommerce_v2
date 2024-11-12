using eCommerce.DataAccess.Repository.IRepository;
using eCommerce.Models.ControllerModels;
using eCommerce.Models.Models;
using eCommerce.Models.ViewModels;
///using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

///alter table yourTableName AUTO_INCREMENT=1;
///truncate table yourTableName;

namespace eCommerce.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(ILogger<WeatherForecastController> logger,
                                    IUnitOfWork unitOfWork/*,
                                    HttpContextAccessor httpContextAccessor*/)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;

            _unitOfWork.CategoryRepository.GetAll();
        }

        ///[HttpGet(Name = "GetCategory")]
        [HttpGet]
        [Route("GetCategory")]
        public IEnumerable<Category> GetCategory()
        {
            return Enumerable.Range(1, 5).Select(index => new Category
            {
                CategoryId = 0,
                Name = "Rose",
                CreatedDateTime = DateTime.Now,
            })
            .ToArray();
        }
       
        [HttpPost]
        [Route("UpsertCategory")]
        ///[HttpPost(Name = "AddCategories")]
        ///[ValidateAntiForgeryToken]
        ///public async Task<ActionResult<string>> Upsert(CategoryList categories)
        ///public IActionResult Upsert(CategoryList categories)
        public async void Upsert(CategoryList categories)
        {
            var createdResource = new { Id = 1, Version = "1.0" };
            string actionName = "UpsertCategory";
            var routeValues = new { id = createdResource.Id, version = createdResource.Version };

            if (ModelState.IsValid)
            {
                IEnumerable<Category> categoryList = _unitOfWork.CategoryRepository.GetAll();
                IEnumerable<SubCategory> subCategoryList = _unitOfWork.SubCategoryRepository.GetAll();

                if (categoryList.Count() == 0 && subCategoryList.Count() == 0)
                {
                    foreach(var categoryObj in categories.Category)
                    {
                        Category category = new Category();
                        category.Name = categoryObj.Category;
                        category.CategoryLinkId = categoryObj.CategoryLinkId;
                        ///category.CreatedDateTime = DateTimeOffset.Now;
                        _unitOfWork.CategoryRepository.Add(category);
                        _unitOfWork.Save();

                        int index=0;
                        foreach (var subCategoryObj in categoryObj.SubCategories/*; var subCategoryLinkIds in categoryObj.SubCategoryLinkIds*/)
                        {
                            SubCategory subCategory = new SubCategory();
                            subCategory.CategoryLinkId = category.CategoryLinkId;
                            subCategory.Name = subCategoryObj;
                            subCategory.SubCategoryLinkId = categoryObj.SubCategoryLinkIds[index++];
                            _unitOfWork.SubCategoryRepository.Add(subCategory);
                            _unitOfWork.Save();
                        }
                    }
                }
            }
        }
    }
}