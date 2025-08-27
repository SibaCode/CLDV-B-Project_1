using ABCRetailDemo.Models;
using ABCRetailDemo.Services;
using Microsoft.AspNetCore.Mvc;

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

        public async Task<IActionResult> Index() => View(await _tableService.GetProductsAsync());

        [HttpGet] 
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(ProductEntity product, IFormFile image)
        {
            if (image != null)
            {
                product.ImageUrl = await _blobService.UploadImageAsync(image);
            }

            product.RowKey = Guid.NewGuid().ToString();
            await _tableService.AddProductAsync(product);
            return RedirectToAction("Index");
        }
    }
}
