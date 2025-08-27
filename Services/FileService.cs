using Azure.Storage.Files.Shares;

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

        public async Task UploadLogAsync(string fileName, string content)
        {
            var directory = _share.GetRootDirectoryClient();
            var file = directory.GetFileClient(fileName);
            await using var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(content));
            await file.CreateAsync(stream.Length);
            await file.UploadAsync(stream);
        }
    }
}
