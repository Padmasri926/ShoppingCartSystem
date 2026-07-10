namespace EshoppingZone.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = default!;
        public string? Description { get; set; }
        public string Category { get; set; } = default!;
        public decimal Price { get; set; }
        public int Stock { get; set; }
    }
}