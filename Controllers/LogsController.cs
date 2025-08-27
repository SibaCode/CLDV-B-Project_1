using ABCRetailDemo.Services;
using Microsoft.AspNetCore.Mvc;

namespace ABCRetailDemo.Controllers
{
    public class LogsController : Controller
    {
        private readonly FileService _fileService;

        public LogsController(FileService fileService)
        {
            _fileService = fileService;
        }

        // Upload form
        [HttpGet]
        public IActionResult Upload() => View();

        // Upload POST
        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                await _fileService.UploadFileAsync(file.FileName, stream);
                TempData["Message"] = $"Uploaded {file.FileName}";
            }
            return RedirectToAction("Upload");
        }

        // List file count
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var count = await _fileService.GetFileCountAsync();
            ViewBag.TotalLogs = count;
            return View();
        }
    }
}
