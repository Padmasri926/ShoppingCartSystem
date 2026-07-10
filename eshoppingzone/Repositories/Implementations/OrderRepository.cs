using EshoppingZone.Data;
using EshoppingZone.Repositories.Interfaces;
using EshoppingZone.Models;
using Microsoft.EntityFrameworkCore;

namespace EshoppingZone.Repositories.Implementations
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(AppDbContext db) : base(db) { }

        public async Task<Order?> GetWithItemsAsync(int orderId)
        {
            return await _db.Orders
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .Include(o => o.Payment)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);
        }

        public async Task<IReadOnlyList<Order>> ListByUserAsync(string userId)
        {
            return await _db.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .Include(o => o.Payment)
                .ToListAsync();
        }
    }
}