namespace EshoppingZone.Services.Interfaces
{
    public interface IWalletService
    {
        Task<decimal> GetBalanceAsync(string userId);
        Task TopUpAsync(string userId, decimal amount);
    }
}
