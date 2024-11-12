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
    public class CountryController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public CountryController(ILogger<WeatherForecastController> logger,
                                    IUnitOfWork unitOfWork/*,
                                    HttpContextAccessor httpContextAccessor*/)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        [Route("UpsertCountry")]
        ///[HttpPost(Name = "AddCategories")]
        ///[ValidateAntiForgeryToken]
        ///public async Task<ActionResult<string>> Upsert(CategoryList categories)
        ///public IActionResult Upsert(CategoryList categories)
        public async void UpsertCountry(CountryList countries)
        {
            var createdResource = new { Id = 1, Version = "1.0" };
            string actionName = "UpsertCountry";
            var routeValues = new { id = createdResource.Id, version = createdResource.Version };

            if (ModelState.IsValid)
            {
                IEnumerable<Country> countryList = _unitOfWork.CountryRepository.GetAll();
                IEnumerable<State> stateList = _unitOfWork.StateRepository.GetAll();
                IEnumerable<City> cityList = _unitOfWork.CityRepository.GetAll();

                if (countryList.Count() == 0 && stateList.Count() == 0
                    && cityList.Count() == 0)
                {
                    foreach (var countryObj in countries.Country)
                    {
                        Country country = new Country();
                        country.CountryLinkId = countryObj.CountryLinkId;
                        country.Name = countryObj.Country;
                        country.FlagCode = countryObj.FlagCode;
                        _unitOfWork.CountryRepository.Add(country);
                        _unitOfWork.Save();

                        foreach (var stateObj in countryObj.States)
                        {
                            State state = new State();
                            state.StateLinkId = stateObj.StateLinkId;
                            state.CountryLinkId = country.CountryLinkId;
                            state.Name = stateObj.State;
                            _unitOfWork.StateRepository.Add(state);
                            _unitOfWork.Save();

                            foreach (var cityObj in stateObj.Cities)
                            {
                                City city = new City();
                                city.CountryLinkId = country.CountryLinkId;
                                city.StateLinkId = state.StateLinkId;
                                city.CityLinkId = cityObj.CityLinkId;
                                city.Name = cityObj.City;
                                _unitOfWork.CityRepository.Add(city);
                            }
                            _unitOfWork.Save();
                        }
                    }
                }
            }
        }

        [HttpGet]
        [Route("GetCountry")]
        public IEnumerable<Country> GetCountries()
        {
            _logger.LogInformation("GetCountries called !!");

            /**
            List<Country> countries = new List<Country>();
            Country country = new Country();
            country.CountryId = 0;
            country.CountryLinkId = 1;
            country.Name = "India";
            country.FlagCode = "IN";

            countries.Add(country);
            return countries;
            */

            return _unitOfWork.CountryRepository.GetAll();
        }

        [HttpGet]
        [Route("GetState")]
        public IEnumerable<State> GetStates()
        {
            _logger.LogInformation("GetStates called !!");
            return _unitOfWork.StateRepository.GetAll();
        }

        [HttpGet]
        [Route("GetCity")]
        public IEnumerable<City> GetCities()
        {
            _logger.LogInformation("GetCities called !!");
            return _unitOfWork.CityRepository.GetAll();
        }

        [HttpGet]
        [Route("GetCitiesOfCountry")]
        public IEnumerable<City> GetCitiesOfCountry([FromQuery]string country)
        {
            ///HttpContext.Session.Clear();

            IEnumerable<City> cityList = new City []{ };
            if (ModelState.IsValid)
            {
                int CountryLinkId = 0;
                IEnumerable<Country> countryList = _unitOfWork.CountryRepository.GetAll(c => c.Name.CompareTo(country) == 0);
                if (countryList.Count() > 0)
                    CountryLinkId = countryList.FirstOrDefault().CountryLinkId;

                cityList = _unitOfWork.CityRepository.GetAll(c => c.CountryLinkId == CountryLinkId);
            }
            return cityList;
        }
    }
}