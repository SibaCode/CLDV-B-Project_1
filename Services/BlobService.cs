using Azure.Storage.Blobs;

namespace ABCRetailDemo.Services
{
    public class BlobService
    {
        private readonly BlobContainerClient _container;

        public BlobService(IConfiguration config)
        {
            var blobService = new BlobServiceClient(config["AzureStorage:ConnectionString"]);
            _container = blobService.GetBlobContainerClient("product-images");
            _container.CreateIfNotExists();
        }

        public async Task<string> UploadImageAsync(IFormFile file)
        {
            var blobClient = _container.GetBlobClient(file.FileName);
            await using var stream = file.OpenReadStream();
            await blobClient.UploadAsync(stream, overwrite: true);
            return blobClient.Uri.ToString();
        }
    }
}
