using FileSystem.Models;
using FileSystem.Repository.IRepository;
using FileSystem.FileSystem;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace FileSystem.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FileSystemController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        private string _folder = FileFolder.folder;

        public FileSystemController(ILogger<WeatherForecastController> logger,
                                      IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet("Product/{fileName}")]
        public async Task<IActionResult> Product(string fileName)
        {
            fileName = _folder + "\\" + fileName.Replace("~", "\\");
            try
            {
                if (System.IO.File.Exists(fileName))
                {
                    //var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                    // return File(fileStream, "application/octet-stream");

                    var provider = new FileExtensionContentTypeProvider();
                    if (!provider.TryGetContentType(fileName, out var contentType))
                    {
                        contentType = "application/octet-stream";
                    }

                    var imageData = await System.IO.File.ReadAllBytesAsync(fileName);

                    var stream = new MemoryStream(imageData);

                    return Ok(stream); // Adjust content type if needed
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

            return Ok();
        }
    }
}