using EshoppingZone.Models;

namespace EshoppingZone.Services.Interfaces
{
    public interface ICartService
    {
        Task<Cart> GetOrCreateCartAsync(string userId);
        Task<Cart> AddProductAsync(string userId, int productId, int quantity);
        Task<Cart> RemoveProductAsync(string userId, int productId, int quantity = int.MaxValue);
        Task<decimal> GetTotalAsync(string userId);
    }
}