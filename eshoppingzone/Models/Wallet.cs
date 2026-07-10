namespace EshoppingZone.Models
{
    public class Wallet
    {
        public int WalletId { get; set; }
        public string UserId { get; set; } = default!;
        public User User { get; set; } = default!;
        public decimal Balance { get; set; }

        public void TopUp(decimal amount)
        {
            if (amount <= 0) throw new InvalidOperationException("Amount must be positive.");
            Balance += amount;
        }

        public void DebitAmount(decimal amount)
        {
            if (amount <= 0) return;
            if (Balance < amount) throw new InvalidOperationException("Insufficient wallet balance.");
            Balance -= amount;
        }
    }
}