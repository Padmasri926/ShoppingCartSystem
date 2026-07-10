using EshoppingZone.Models;

namespace EshoppingZone.Repositories.Interfaces
{
    public interface ICartRepository : IGenericRepository<Cart>
    {
        Task<Cart?> GetCartWithItemsAsync(string userId);
    }
}