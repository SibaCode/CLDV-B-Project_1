using Azure.Storage.Queues;

namespace ABCRetailDemo.Services
{
    public class QueueService
    {
        private readonly QueueClient _queue;

        public QueueService(IConfiguration config)
        {
             var connectionString = config["AzureStorage:ConnectionString"];
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException("AzureStorage:ConnectionString is missing from configuration.");

            _queue = new QueueClient(connectionString, "orders");
            _queue.CreateIfNotExists();
        }

        public async Task EnqueueOrderAsync(string message) =>
            await _queue.SendMessageAsync(message);
    }
}
