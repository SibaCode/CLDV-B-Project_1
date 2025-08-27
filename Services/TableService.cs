using Azure.Data.Tables;
using ABCRetailDemo.Models;

namespace ABCRetailDemo.Services
{
    public class TableService
    {
        private readonly TableServiceClient _serviceClient;
        private readonly TableClient _customerTable;
        private readonly TableClient _productTable;
        private readonly TableClient _orderTable;

        public TableService(IConfiguration config)
        {
            var connectionString = config["AzureStorage:ConnectionString"];
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException("AzureStorage:ConnectionString is missing in appsettings.json");
            var serviceClient = new TableServiceClient(connectionString);

            _orderTable = serviceClient.GetTableClient("Orders");

            _serviceClient = new TableServiceClient(connectionString);
   _orderTable = serviceClient.GetTableClient("Orders");
            _orderTable.CreateIfNotExists();
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
public async Task AddOrderAsync(OrderEntity order) =>
            await _orderTable.AddEntityAsync(order);

        public async Task<List<OrderEntity>> GetOrdersAsync()
        {
            var orders = new List<OrderEntity>();
            await foreach (var o in _orderTable.QueryAsync<OrderEntity>())
            {
                orders.Add(o);
            }
            return orders;
        }
        public async Task<ProductEntity> GetProductAsync(string partitionKey, string rowKey)
{
    try
    {
        var entity = await _productTable.GetEntityAsync<ProductEntity>(partitionKey, rowKey);
        return entity.Value;
    }
    catch
    {
        return null;
    }
}

     
public async Task UpdateProductAsync(ProductEntity product)
{
    await _productTable.UpdateEntityAsync(product, product.ETag, Azure.Data.Tables.TableUpdateMode.Replace);
}

public async Task DeleteProductAsync(string partitionKey, string rowKey)
{
    try
    {
        var entity = await _productTable.GetEntityAsync<ProductEntity>(partitionKey, rowKey);
        if (entity != null)
        {
            await _productTable.DeleteEntityAsync(partitionKey, rowKey);
        }
    }
    catch (Exception ex)
    {
        // Log the exception (ex) if necessary
        throw;
    }
}


    }
}
