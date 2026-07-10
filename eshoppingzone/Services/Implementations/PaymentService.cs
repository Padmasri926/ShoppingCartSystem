using EshoppingZone.Data;
using EshoppingZone.Models;
using EshoppingZone.Repositories.Interfaces;
using EshoppingZone.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EshoppingZone.Services.Implementations
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _payments;
        private readonly IOrderRepository _orders;
        private readonly AppDbContext _db;

        public PaymentService(IPaymentRepository payments, IOrderRepository orders, AppDbContext db)
        {
            _payments = payments;
            _orders = orders;
            _db = db;
        }

        public async Task<Payment> MakePaymentAsync(int orderId, string paymentMode, decimal amount, int? walletId)
        {
            var order = await _orders.GetWithItemsAsync(orderId) ?? throw new KeyNotFoundException("Order not found.");
            var total = order.Items.Sum(i => i.UnitPrice * i.Quantity);

            if (amount != total) throw new InvalidOperationException($"Amount mismatch. Expected {total}.");

            var payment = new Payment { OrderId = orderId, PaymentMode = paymentMode, Amount = total, Status = "Pending" };

            if (string.Equals(paymentMode, "Wallet", StringComparison.OrdinalIgnoreCase))
            {
                var wallet = await _db.Wallets.FirstOrDefaultAsync(w => w.UserId == order.UserId);
                if (wallet is null) throw new InvalidOperationException("Wallet not found.");
                wallet.DebitAmount(total);
                payment.Status = "Completed";
            }

            await _payments.AddAsync(payment);

            // Record a transaction line
            payment.Transactions.Add(new PaymentTransaction { PaymentId = payment.PaymentId, Amount = total });

            _payments.Update(payment);
            await _payments.SaveChangesAsync();

            // Mark order as paid
            order.Status = "Paid";
            _orders.Update(order);
            await _orders.SaveChangesAsync();

            return payment;
        }
    }
}