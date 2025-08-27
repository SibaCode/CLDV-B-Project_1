using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace ABCRetailDemo.Services
{
    public class BlobService
    {
        private readonly BlobContainerClient _container;

        public BlobService(IConfiguration config)
        {
            var conn = config["AzureStorage:ConnectionString"];
            var blobServiceClient = new BlobServiceClient(conn);

            // Container for product images
            _container = blobServiceClient.GetBlobContainerClient("productimages");
            _container.CreateIfNotExists();
        }

        public async Task<string> UploadImageAsync(IFormFile file)
        {
            // Upload the blob
            var blobClient = _container.GetBlobClient(file.FileName);
            await using var stream = file.OpenReadStream();
            await blobClient.UploadAsync(stream, overwrite: true);

            // Generate a SAS URL valid for 1 day
            var sasBuilder = new BlobSasBuilder
            {
                BlobContainerName = _container.Name,
                BlobName = file.FileName,
                Resource = "b",
                ExpiresOn = DateTimeOffset.UtcNow.AddDays(1)
            };
            sasBuilder.SetPermissions(BlobSasPermissions.Read);

            var sasUri = blobClient.GenerateSasUri(sasBuilder);
            return sasUri.ToString();
        }
    }
}
