using ABCRetailDemo.Models;
using ABCRetailDemo.Services;
using Microsoft.AspNetCore.Mvc;
using Azure;

namespace ABCRetailDemo.Controllers
{
    public class OrderController : Controller
    {
        private readonly TableService _tableService;
        private readonly QueueService _queueService;

        public OrderController(TableService tableService, QueueService queueService)
        {
            _tableService = tableService;
            _queueService = queueService;
        }

        // List Orders
        public async Task<IActionResult> Index()
        {
            var orders = await _tableService.GetOrdersAsync();
            return View(orders);
        }

        // GET: Create Order
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Customers = await _tableService.GetCustomersAsync();
            ViewBag.Products = await _tableService.GetProductsAsync();
            return View();
        }

        // POST: Create Order
       [HttpPost]
public async Task<IActionResult> Create(string customerName, string productName, int quantity, decimal price)
{
    if (string.IsNullOrWhiteSpace(customerName) || string.IsNullOrWhiteSpace(productName) || quantity <= 0)
    {
        ModelState.AddModelError("", "All fields are required and quantity must be > 0.");

        // Repopulate dropdowns
        ViewBag.Customers = await _tableService.GetCustomersAsync();
        ViewBag.Products = await _tableService.GetProductsAsync();

        return View();
    }

    var order = new OrderEntity
    {
        RowKey = Guid.NewGuid().ToString(),
        CustomerName = customerName,
        ProductName = productName,
        Quantity = quantity,
        Price = price,
        OrderDate = DateTime.UtcNow,
        ETag = Azure.ETag.All
    };

    await _tableService.AddOrderAsync(order);

    // Add message to queue
    await _queueService.EnqueueOrderAsync($"{order.CustomerName}|{order.ProductName}|{order.Quantity}|{order.OrderDate}");

    return RedirectToAction("Index");
}

    }
}
