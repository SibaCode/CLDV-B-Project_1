using Azure.Storage.Files.Shares;
using System.Text;

namespace ABCRetailDemo.Services
{
    public class FileService
    {
        private readonly ShareClient _shareClient;

        public FileService(string connectionString, string shareName)
        {
            _shareClient = new ShareClient(connectionString, shareName);
            _shareClient.CreateIfNotExists();
        }

        public async Task UploadFileAsync(string fileName, string content)
        {
            var directory = _shareClient.GetRootDirectoryClient();
            var file = directory.GetFileClient(fileName);
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
            await file.CreateAsync(stream.Length);
            await file.UploadAsync(stream);
        }
    }
}
