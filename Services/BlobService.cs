using Azure.Storage.Blobs;

namespace ABCRetailDemo.Services
{
    public class BlobService
    {
        private readonly BlobContainerClient _containerClient;

        public BlobService(string connectionString, string containerName)
        {
            var client = new BlobServiceClient(connectionString);
            _containerClient = client.GetBlobContainerClient(containerName);
            _containerClient.CreateIfNotExists();
        }

        public async Task<string> UploadFileAsync(IFormFile file)
        {
            var blobClient = _containerClient.GetBlobClient(file.FileName);
            using var stream = file.OpenReadStream();
            await blobClient.UploadAsync(stream, true);
            return file.FileName; // store blob name
        }

        public string GetBlobUri(string blobName)
        {
            return _containerClient.GetBlobClient(blobName).Uri.ToString();
        }
    }
}
