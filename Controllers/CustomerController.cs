using ABCRetailDemo.Models;
using ABCRetailDemo.Services;
using Microsoft.AspNetCore.Mvc;

namespace ABCRetailDemo.Controllers
{
    public class CustomersController : Controller
    {
        private readonly TableService _tableService;

        public CustomersController(TableService tableService)
        {
            _tableService = tableService;
        }

        public async Task<IActionResult> Index() => View(await _tableService.GetCustomersAsync());

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Customer customer)
        {
            if (!ModelState.IsValid) return View(customer);
            await _tableService.AddCustomerAsync(customer);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string rowKey)
        {
            await _tableService.DeleteCustomerAsync(rowKey);
            return RedirectToAction(nameof(Index));
        }
    }
}
