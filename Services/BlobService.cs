using ABCRetailDemo.Models;
using ABCRetailDemo.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ABCRetailDemo.Controllers
{
    public class ProductController : Controller
    {
        private readonly TableService _tableService;
        private readonly BlobService _blobService;

        public ProductController(TableService tableService, BlobService blobService)
        {
            _tableService = tableService;
            _blobService = blobService;
        }

        // List all products
        public async Task<IActionResult> Index()
        {
            var products = await _tableService.GetProductsAsync();
            return View(products);
        }

        // Create
        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(ProductEntity product, IFormFile? image)
        {
            if (image != null)
            {
                // Upload image and get SAS URL
                product.ImageUrl = await _blobService.UploadImageAsync(image);
            }

            product.RowKey = Guid.NewGuid().ToString();
            product.PartitionKey = "Products";

            await _tableService.AddProductAsync(product);

            return RedirectToAction("Index");
        }

        // Edit
        [HttpGet]
        public async Task<IActionResult> Edit(string rowKey)
        {
            var product = await _tableService.GetProductAsync("Products", rowKey);
            if (product == null) return NotFound();
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProductEntity product, IFormFile? image)
        {
            if (image != null)
            {
                product.ImageUrl = await _blobService.UploadImageAsync(image);
            }

            await _tableService.UpdateProductAsync(product);
            return RedirectToAction("Index");
        }

        // Delete
        [HttpGet]
        public async Task<IActionResult> Delete(string rowKey)
        {
            var product = await _tableService.GetProductAsync("Products", rowKey);
            if (product == null) return NotFound();
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(string rowKey)
        {
            await _tableService.DeleteProductAsync("Products", rowKey);
            return RedirectToAction("Index");
        }
    }
}
