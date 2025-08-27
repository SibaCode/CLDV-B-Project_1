using ABCRetailDemo.Models;
using ABCRetailDemo.Services;
using Microsoft.AspNetCore.Mvc;

namespace ABCRetailDemo.Controllers
{
    public class ProductsController : Controller
    {
        private readonly TableService _tableService;
        private readonly BlobService _blobService;

        public ProductsController(TableService tableService, BlobService blobService)
        {
            _tableService = tableService;
            _blobService = blobService;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _tableService.GetProductsAsync();

            foreach (var p in products)
            {
                if (!string.IsNullOrEmpty(p.ImageUrl))
                    p.ImageUrl = _blobService.GetBlobUri(p.ImageUrl);
            }

            return View(products);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product, IFormFile image)
        {
            if (image != null && image.Length > 0)
            {
                product.ImageUrl = await _blobService.UploadFileAsync(image);
            }

            await _tableService.AddProductAsync(product);
            return RedirectToAction(nameof(Index));
        }
    }
}
