using Azure.Storage.Queues;

namespace ABCRetailDemo.Services
{
    public class QueueService
    {
        private readonly QueueClient _queue;

        public QueueService(IConfiguration config)
        {
            _queue = new QueueClient(config["AzureStorage:ConnectionString"], "orders");
            _queue.CreateIfNotExists();
        }

        public async Task EnqueueOrderAsync(string message) =>
            await _queue.SendMessageAsync(message);
    }
}
