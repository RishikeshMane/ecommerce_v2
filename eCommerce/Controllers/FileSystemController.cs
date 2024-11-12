#define FILESYSTEM

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using eCommerce.DataAccess.Repository.IRepository;
using eCommerce.FileSystem;

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

        private readonly IUnitOfWork _unitOfWork;

        private string _folder = eCommerce.FileSystem.FileSystem.folder;

        public FileSystemController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

#if FILESYSTEM
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
#endif
    }
}