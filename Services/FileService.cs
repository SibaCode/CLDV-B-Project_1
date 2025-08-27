using Azure.Storage.Files.Shares;
using System.IO;

namespace ABCRetailDemo.Services
{
    public class FileService
    {
        private readonly ShareClient _share;

        public FileService(IConfiguration config)
        {
            _share = new ShareClient(config["AzureStorage:ConnectionString"], "logs");
            _share.CreateIfNotExists();
        }

        // Upload a log file
        public async Task UploadLogAsync(string fileName, string content)
        {
            var directory = _share.GetRootDirectoryClient();
            var file = directory.GetFileClient(fileName);

            await using var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(content));
            await file.CreateAsync(stream.Length);
            await file.UploadAsync(stream);
        }

        // List all files in the share
        public async Task<List<string>> ListFilesAsync()
        {
            var directory = _share.GetRootDirectoryClient();
            var files = new List<string>();

            await foreach (var item in directory.GetFilesAndDirectoriesAsync())
            {
                if (!item.IsDirectory)
                    files.Add(item.Name);
            }

            return files;
        }
    }
}
