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
            var shareName = config["AzureFiles:ShareName"];
 if (string.IsNullOrEmpty(conn))
                throw new ArgumentNullException(nameof(conn), "Azure Storage connection string is missing.");

            var blobServiceClient = new BlobServiceClient(conn);

            // Container for product images (private)
            _container = blobServiceClient.GetBlobContainerClient("productimages");
            _container.CreateIfNotExists(Azure.Storage.Blobs.Models.PublicAccessType.None);
        }

        /// <summary>
        /// Uploads a file and returns a SAS URL valid for 7 days
        /// </summary>
        public async Task<string> UploadImageAsync(IFormFile file)
        {
            var blobClient = _container.GetBlobClient(file.FileName);
            await using var stream = file.OpenReadStream();
            await blobClient.UploadAsync(stream, overwrite: true);

            var sasBuilder = new BlobSasBuilder
            {
                BlobContainerName = _container.Name,
                BlobName = file.FileName,
                Resource = "b",
                ExpiresOn = DateTimeOffset.UtcNow.AddDays(7) // longer validity
            };

            sasBuilder.SetPermissions(BlobSasPermissions.Read);

            return blobClient.GenerateSasUri(sasBuilder).ToString();
        }
    }
}
