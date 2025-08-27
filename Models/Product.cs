namespace ABCRetailDemo.Models
{
    public class Product
    {
        public string Id { get; set; }       // Maps to RowKey
        public string Name { get; set; }     // Maps to ProductEntity.Name
        public string Description { get; set; } // Maps to ProductEntity.Description
        public decimal Price { get; set; }   // Maps to ProductEntity.Price
        public string ImageUrl { get; set; } // Maps to ProductEntity.ImageUrl
    }
}
