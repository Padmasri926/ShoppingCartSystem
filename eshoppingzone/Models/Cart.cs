namespace EshoppingZone.Models
{
    public class Cart
    {
        public int CartId { get; set; }
        public string UserId { get; set; } = default!;
        public User User { get; set; } = default!;
        public ICollection<CartItem> Items { get; set; } = new List<CartItem>();
    }
}