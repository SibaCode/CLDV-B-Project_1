using Azure.Storage.Queues;

namespace ABCRetailDemo.Services
{
    public class QueueService
    {
        private readonly QueueClient _queueClient;

        public QueueService(string connectionString, string queueName)
        {
            _queueClient = new QueueClient(connectionString, queueName);
            _queueClient.CreateIfNotExists();
        }

        public async Task EnqueueMessageAsync(string message)
        {
            await _queueClient.SendMessageAsync(message);
        }

        public async Task<List<string>> GetMessagesAsync()
        {
            var list = new List<string>();
            var messages = await _queueClient.ReceiveMessagesAsync(32);
            foreach (var msg in messages.Value)
                list.Add(msg.MessageText);
            return list;
        }
    }
}
