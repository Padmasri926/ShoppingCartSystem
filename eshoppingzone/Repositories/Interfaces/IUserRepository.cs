using EshoppingZone.Models;

namespace EshoppingZone.Repositories.Interfaces
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetWithCartAsync(string userId);
        Task<User?> GetWithWalletAsync(string userId);
    }
}