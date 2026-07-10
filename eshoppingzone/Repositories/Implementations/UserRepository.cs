using EshoppingZone.Data;
using EshoppingZone.Repositories.Interfaces;
using EshoppingZone.Models;
using Microsoft.EntityFrameworkCore;

namespace EshoppingZone.Repositories.Implementations
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(AppDbContext db) : base(db) { }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> GetWithCartAsync(string userId)
        {
            return await _db.Users
                .Include(u => u.Cart)
                .ThenInclude(c => c!.Items)
                .FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<User?> GetWithWalletAsync(string userId)
        {
            return await _db.Users
                .Include(u => u.Wallet)
                .FirstOrDefaultAsync(u => u.Id == userId);
        }
    }
}