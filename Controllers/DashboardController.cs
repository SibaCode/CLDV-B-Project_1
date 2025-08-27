using ABCRetailDemo.Services;
using Microsoft.AspNetCore.Mvc;

namespace ABCRetailDemo.Controllers
{
    public class DashboardController : Controller
    {
        private readonly TableService _tableService;
        private readonly BlobService _blobService;
        private readonly QueueService _queueService;

        public DashboardController(TableService tableService, BlobService blobService, QueueService queueService)
        {
            _tableService = tableService;
            _blobService = blobService;
            _queueService = queueService;
        }

        public async Task<IActionResult> Index()
        {
            var customers = await _tableService.GetCustomersAsync();
            var products = await _tableService.GetProductsAsync();
            // For demo, we can just show number of orders in queue if implemented
            var totalOrders = 0; 
            var totalImages = 0; // Later you can implement Blob listing

            ViewBag.TotalCustomers = customers.Count;
            ViewBag.TotalProducts = products.Count;
            ViewBag.TotalOrders = totalOrders;
            ViewBag.PendingOrders = totalOrders;
            ViewBag.TotalProductImages = totalImages;

            return View();
        }
    }
}
