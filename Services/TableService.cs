using Azure.Data.Tables;
using ABCRetailDemo.Models;

namespace ABCRetailDemo.Services
{
    public class TableService
    {
        private readonly TableServiceClient _serviceClient;
        private readonly TableClient _customerTable;
        private readonly TableClient _productTable;

        public TableService(IConfiguration config)
        {
            var connectionString = config["AzureStorage:ConnectionString"];
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException("AzureStorage:ConnectionString is missing in appsettings.json");

            _serviceClient = new TableServiceClient(connectionString);

            // Initialize tables
            _customerTable = _serviceClient.GetTableClient("Customers");
            _customerTable.CreateIfNotExists();

            _productTable = _serviceClient.GetTableClient("Products");
            _productTable.CreateIfNotExists();
        }

        public async Task AddCustomerAsync(CustomerEntity customer) =>
            await _customerTable.AddEntityAsync(customer);

        public async Task AddProductAsync(ProductEntity product) =>
            await _productTable.AddEntityAsync(product);

        public async Task<List<CustomerEntity>> GetCustomersAsync()
        {
            var list = new List<CustomerEntity>();
            await foreach (var customer in _customerTable.QueryAsync<CustomerEntity>())
            {
                list.Add(customer);
            }
            return list;
        }

        public async Task<List<ProductEntity>> GetProductsAsync()
        {
            var list = new List<ProductEntity>();
            await foreach (var product in _productTable.QueryAsync<ProductEntity>())
            {
                list.Add(product);
            }
            return list;
        }
    }
}
