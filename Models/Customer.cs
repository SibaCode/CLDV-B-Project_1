namespace ABCRetailDemo.Models
{
    public class Customer
    {
        public string Id { get; set; }   // This will map to RowKey from Azure Table
        public string Name { get; set; } // Maps to CustomerEntity.Name
        public string Email { get; set; } // Maps to CustomerEntity.Email
        public string Phone { get; set; } // Maps to CustomerEntity.Phone
    }
}
