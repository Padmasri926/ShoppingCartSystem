using EshoppingZone.Data;
using EshoppingZone.Repositories.Interfaces;
using EshoppingZone.Models;
using Microsoft.EntityFrameworkCore;

namespace EshoppingZone.Repositories.Implementations
{
    public class CartRepository : GenericRepository<Cart>, ICartRepository
    {
        public CartRepository(AppDbContext db) : base(db) { }

        public async Task<Cart?> GetCartWithItemsAsync(string userId)
        {
            return await _db.Carts
                .Include(c => c.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }
    }
}