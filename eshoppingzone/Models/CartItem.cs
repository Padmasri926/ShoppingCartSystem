namespace EshoppingZone.Models
{
    public class CartItem
    {
        public int CartItemId { get; set; }             // PK
        public int CartId { get; set; }                 // FK
        public Cart Cart { get; set; } = default!;
        public int ProductId { get; set; }              // FK
        public Product Product { get; set; } = default!;
        public int Quantity { get; set; }
    }
}