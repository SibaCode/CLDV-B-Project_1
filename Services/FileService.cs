using Azure.Storage.Files.Shares;
using Azure.Storage.Files.Shares.Models;

namespace ABCRetailDemo.Services
{
    public class FileService
    {
        private readonly ShareClient _shareClient;

        public FileService(IConfiguration config)
        {
            
            var connectionString = config["AzureFiles:ConnectionString"];
            var shareName = config["AzureFiles:ShareName"]; // e.g., "logs"
            if (string.IsNullOrEmpty(connectionString) || string.IsNullOrEmpty(shareName))
                throw new ArgumentNullException("AzureFiles connection string or share name is missing.");

            _shareClient = new ShareClient(connectionString, shareName);
            _shareClient.CreateIfNotExists();
        }

        // Count files in the root or a folder
        public async Task<int> GetFileCountAsync(string directoryName = "")
        {
            ShareDirectoryClient directoryClient = string.IsNullOrEmpty(directoryName)
                ? _shareClient.GetRootDirectoryClient()
                : _shareClient.GetDirectoryClient(directoryName);

            int count = 0;
            await foreach (ShareFileItem fileItem in directoryClient.GetFilesAndDirectoriesAsync())
            {
                if (!fileItem.IsDirectory)
                    count++;
            }
            return count;
        }

        // Optional: Upload a log/file
        public async Task UploadFileAsync(string fileName, Stream content)
        {
            var root = _shareClient.GetRootDirectoryClient();
            var fileClient = root.GetFileClient(fileName);
            await fileClient.CreateAsync(content.Length);
            await fileClient.UploadAsync(content);
        }
    }
}
