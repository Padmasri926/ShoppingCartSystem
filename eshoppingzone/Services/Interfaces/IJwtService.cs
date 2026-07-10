using EshoppingZone.Models;

namespace EshoppingZone.Services.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(User user);
    }
}
