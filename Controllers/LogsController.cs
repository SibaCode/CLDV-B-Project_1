using ABCRetailDemo.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace ABCRetailDemo.Controllers
{
    public class LogsController : Controller
    {
        private readonly FileService _fileService;

        public LogsController(FileService fileService)
        {
            _fileService = fileService;
        }

        // Display all logs
        public async Task<IActionResult> Index()
        {
            var logs = await _fileService.ListFilesAsync();
            ViewBag.Logs = logs;
            return View();
        }

        // GET: Upload log
        [HttpGet]
        public IActionResult Upload()
        {
            return View();
        }

        // POST: Upload log content as a file
        [HttpPost]
        public async Task<IActionResult> Upload(string fileName, string logContent)
        {
            if (string.IsNullOrWhiteSpace(fileName) || string.IsNullOrWhiteSpace(logContent))
            {
                ViewBag.Message = "File name and log content are required.";
                return View();
            }

            var bytes = Encoding.UTF8.GetBytes(logContent);
            using var stream = new MemoryStream(bytes);

            // Upload to Azure File Storage
            await _fileService.UploadFileAsync(fileName, stream);

            ViewBag.Message = $"Log '{fileName}' uploaded successfully!";
            return View();
        }
    }
}
