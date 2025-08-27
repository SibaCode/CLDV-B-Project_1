namespace ABCRetailDemo.Models
{
    public class OrderMessage
    {
        public string OrderId { get; set; }
        public string CustomerId { get; set; }
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; } = "Pending";
    }
}
