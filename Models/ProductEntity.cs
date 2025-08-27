using Azure;
using Azure.Data.Tables;

namespace ABCRetailDemo.Models
{
    public class ProductEntity : ITableEntity
    {
        public string PartitionKey { get; set; } = "Products"; // Use a fixed partition key
        public string RowKey { get; set; }                     // Unique ID
        public string ProductName { get; set; }
        public double Price { get; set; }                     // Must be double or decimal
        public string Description { get; set; }
        public string ImageUrl { get; set; }

        // Table Storage system properties
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
    }
}
