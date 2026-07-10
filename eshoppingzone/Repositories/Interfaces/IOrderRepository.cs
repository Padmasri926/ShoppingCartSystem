using EshoppingZone.Models;

namespace EshoppingZone.Repositories.Interfaces
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<Order?> GetWithItemsAsync(int orderId);
        Task<IReadOnlyList<Order>> ListByUserAsync(string userId);
    }
}