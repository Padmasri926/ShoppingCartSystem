using EshoppingZone.Data;
using EshoppingZone.Repositories.Interfaces;
using EshoppingZone.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EshoppingZone.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _db;

        public UserService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<bool> UserExistsAsync(string userId)
        {
            return await _db.Users.AnyAsync(u => u.Id == userId);
        }
    }
}