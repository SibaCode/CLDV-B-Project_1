using Microsoft.AspNetCore.Mvc;
using ABCRetailDemo.Services;

public class LogsController : Controller
{
    private readonly FileService _fileService;

    public LogsController(FileService fileService)
    {
        _fileService = fileService;
    }

    // GET: Upload form
    [HttpGet]
    public IActionResult Upload() => View();

    // POST: Upload log
    [HttpPost]
    public async Task<IActionResult> Upload(string fileName, string logContent)
    {
        if (!string.IsNullOrWhiteSpace(fileName) && !string.IsNullOrWhiteSpace(logContent))
        {
            await _fileService.UploadLogAsync(fileName, logContent);
            ViewBag.Message = "File uploaded successfully!";
        }
        return View();
    }

    // GET: List all logs
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var logs = await _fileService.ListFilesAsync();
        ViewBag.Logs = logs;
        return View();
    }
}
