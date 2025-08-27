using Azure.Data.Tables;
using ABCRetailDemo.Models;

namespace ABCRetailDemo.Services
{
    public class TableService
    {
        private readonly TableClient _productTable;
        private readonly TableClient _customerTable;

        public TableService(string connectionString)
        {
            var serviceClient = new TableServiceClient(connectionString);

            // Products table
            _productTable = serviceClient.GetTableClient("Products");
            _productTable.CreateIfNotExists();

            // Customers table
            _customerTable = serviceClient.GetTableClient("Customers");
            _customerTable.CreateIfNotExists();
        }

        // -------------------- Product Methods --------------------
        public async Task AddProductAsync(Product product)
        {
            product.PartitionKey = "Product";
            product.RowKey = Guid.NewGuid().ToString();
            await _productTable.AddEntityAsync(product);
        }

        public async Task<List<Product>> GetProductsAsync()
        {
            var result = new List<Product>();
            await foreach (var p in _productTable.QueryAsync<Product>(e => e.PartitionKey == "Product"))
            {
                result.Add(p);
            }
            return result;
        }

        public async Task DeleteProductAsync(string rowKey)
        {
            await _productTable.DeleteEntityAsync("Product", rowKey);
        }

        // -------------------- Customer Methods --------------------
        public async Task AddCustomerAsync(Customer customer)
        {
            customer.PartitionKey = "Customer";
            customer.RowKey = Guid.NewGuid().ToString();
            await _customerTable.AddEntityAsync(customer);
        }

        public async Task<List<Customer>> GetCustomersAsync()
        {
            var result = new List<Customer>();
            await foreach (var c in _customerTable.QueryAsync<Customer>(e => e.PartitionKey == "Customer"))
            {
                result.Add(c);
            }
            return result;
        }

        public async Task DeleteCustomerAsync(string rowKey)
        {
            await _customerTable.DeleteEntityAsync("Customer", rowKey);
        }
    }
}
