using ABCRetailDemo.Services;
using Microsoft.AspNetCore.Mvc;

namespace ABCRetailDemo.Controllers
{
    public class DashboardController : Controller
    {
        private readonly TableService _tableService;
        private readonly FileService _fileService;

        public DashboardController(TableService tableService, FileService fileService)
        {
            _tableService = tableService;
            _fileService = fileService;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.TotalCustomers = (await _tableService.GetCustomersAsync()).Count;
            ViewBag.TotalProducts = (await _tableService.GetProductsAsync()).Count;
            ViewBag.TotalOrders = (await _tableService.GetOrdersAsync()).Count;
            ViewBag.TotalLogs = await _fileService.GetFileCountAsync();

            return View();
        }
    }
}
