namespace EshoppingZone.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public string UserId { get; set; } = default!;
        public User User { get; set; } = default!;
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public string Status { get; set; } = "Created";
        public decimal TotalAmount { get; set; }

        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
        public Payment? Payment { get; set; }
    }
}