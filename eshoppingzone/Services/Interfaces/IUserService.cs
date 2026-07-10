namespace EshoppingZone.Services.Interfaces
{
    public interface IUserService
    {
        Task<bool> UserExistsAsync(string userId);
    }
}