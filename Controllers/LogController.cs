using ABCRetailDemo.Services;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace ABCRetailDemo.Controllers
{
    public class LogController : Controller
    {
        private readonly FileService _fileService;

        public LogController(FileService fileService) => _fileService = fileService;

        [HttpGet] 
        public IActionResult Upload() => View();

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile logFile)
        {
            if (logFile != null)
            {
                using var reader = new StreamReader(logFile.OpenReadStream());
                var content = await reader.ReadToEndAsync();
                await _fileService.UploadLogAsync(logFile.FileName, content);
            }
            return RedirectToAction("Upload");
        }
    }
}
