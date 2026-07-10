namespace EshoppingZone.Models
{
    public class PaymentTransaction
    {
        public int PaymentTransactionId { get; set; }   // PK
        public int PaymentId { get; set; }              // FK
        public Payment Payment { get; set; } = default!;
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;
    }
}