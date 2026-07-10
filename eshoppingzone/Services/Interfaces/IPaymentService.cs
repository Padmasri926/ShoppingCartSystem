using EshoppingZone.Models;

namespace EshoppingZone.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<Payment> MakePaymentAsync(int orderId, string paymentMode, decimal amount, int? walletId);
    }
}