using EshoppingZone.Models;

namespace EshoppingZone.Services.Interfaces
{
    public interface IOrderService
    {
        Task<Order> PlaceOrderAsync(string userId, string paymentMode);
        Task<Order?> GetOrderAsync(int orderId);
        Task<decimal> CalculateTotalAsync(int orderId);
        Task<IReadOnlyList<Order>> GetUserOrdersAsync(string userId);
    }
}