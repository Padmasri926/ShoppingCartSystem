namespace EshoppingZone.Models
{
    public class Payment
    {
        public int PaymentId { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; } = default!;
        public string PaymentMode { get; set; } = "COD"; // Wallet or COD
        public decimal Amount { get; set; }
        public string Status { get; set; } = "Pending";

        public ICollection<PaymentTransaction> Transactions { get; set; } = new List<PaymentTransaction>();
    }
}