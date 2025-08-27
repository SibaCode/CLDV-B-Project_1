using ABCRetailDemo.Models;
using ABCRetailDemo.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace ABCRetailDemo.Controllers
{
    public class OrderController : Controller
    {
        private readonly QueueService _queueService;

        public OrderController(QueueService queueService) => _queueService = queueService;

        [HttpGet] 
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(OrderMessage order)
        {
            order.OrderId = Guid.NewGuid().ToString();
            string messageJson = JsonSerializer.Serialize(order);
            await _queueService.EnqueueOrderAsync(messageJson);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Index()
        {
            // For demo purposes, we just list queue messages
            // You can extend this to deserialize JSON messages
            return View(); 
        }
    }
}
