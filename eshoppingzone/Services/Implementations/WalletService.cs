using EshoppingZone.Data;
using EshoppingZone.Models;
using EshoppingZone.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EshoppingZone.Services.Implementations
{
    public class WalletService : IWalletService
    {
        private readonly AppDbContext _db;

        public WalletService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<decimal> GetBalanceAsync(string userId)
        {
            var wallet = await _db.Wallets.FirstOrDefaultAsync(w => w.UserId == userId);
            if (wallet == null)
            {
                wallet = new Wallet { UserId = userId, Balance = 0 };
                _db.Wallets.Add(wallet);
                await _db.SaveChangesAsync();
            }
            return wallet.Balance;
        }

        public async Task TopUpAsync(string userId, decimal amount)
        {
            var wallet = await _db.Wallets.FirstOrDefaultAsync(w => w.UserId == userId);
            if (wallet == null)
            {
                wallet = new Wallet { UserId = userId, Balance = 0 };
                _db.Wallets.Add(wallet);
            }
            wallet.TopUp(amount);
            await _db.SaveChangesAsync();
        }
    }
}
