using ABCRetailDemo.Models;
using ABCRetailDemo.Services;
using Microsoft.AspNetCore.Mvc;

namespace ABCRetailDemo.Controllers
{
    public class CustomersController : Controller
    {
        private readonly TableService _tableService;

        public CustomersController(TableService tableService) => _tableService = tableService;

        public async Task<IActionResult> Index() => View(await _tableService.GetCustomersAsync());

        [HttpGet] public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(CustomerEntity customer)
        {
            customer.RowKey = Guid.NewGuid().ToString();
            await _tableService.AddCustomerAsync(customer);
            return RedirectToAction("Index");
        }
    }
}
