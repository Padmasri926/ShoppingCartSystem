namespace EshoppingZone.Models
{
    public class OrderItem
    {
        public int OrderItemId { get; set; }            // PK
        public int OrderId { get; set; }                // FK
        public Order Order { get; set; } = default!;
        public int ProductId { get; set; }              // FK
        public Product Product { get; set; } = default!;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }          // snapshot price
    }
}