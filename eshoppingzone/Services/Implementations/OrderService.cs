using EshoppingZone.Data;
using EshoppingZone.Models;
using EshoppingZone.Repositories.Interfaces;
using EshoppingZone.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EshoppingZone.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orders;
        private readonly ICartRepository _carts;
        private readonly AppDbContext _db;

        public OrderService(IOrderRepository orders, ICartRepository carts, AppDbContext db)
        {
            _orders = orders;
            _carts = carts;
            _db = db;
        }

        public async Task<Order> PlaceOrderAsync(string userId, string paymentMode)
        {
            var cart = await _carts.GetCartWithItemsAsync(userId) ?? throw new InvalidOperationException("Cart not found.");
            if (!cart.Items.Any()) throw new InvalidOperationException("Cart is empty.");

            var totalAmount = cart.Items.Sum(i => i.Quantity * i.Product.Price);

            var order = new Order { UserId = userId, Status = "Created", TotalAmount = totalAmount };
            foreach (var item in cart.Items)
            {
                order.Items.Add(new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.Product.Price
                });
            }

            await _orders.AddAsync(order);

            // Process payment
            var payment = new Payment
            {
                OrderId = order.OrderId,
                PaymentMode = paymentMode,
                Amount = totalAmount,
                Status = "Pending"
            };

            if (paymentMode == "Wallet")
            {
                var wallet = await _db.Wallets.FirstOrDefaultAsync(w => w.UserId == userId);
                if (wallet == null || wallet.Balance < totalAmount)
                    throw new InvalidOperationException("Insufficient wallet balance.");

                wallet.DebitAmount(totalAmount);
                payment.Status = "Completed";
                order.Status = "Confirmed";
            }
            else if (paymentMode == "COD")
            {
                payment.Status = "Pending";
                order.Status = "Confirmed";
            }

            _db.Payments.Add(payment);
            await _db.SaveChangesAsync();

            // Clear the cart
            cart.Items.Clear();
            _carts.Update(cart);
            await _carts.SaveChangesAsync();

            return order;
        }

        public Task<Order?> GetOrderAsync(int orderId) => _orders.GetWithItemsAsync(orderId);

        public async Task<decimal> CalculateTotalAsync(int orderId)
        {
            var order = await _orders.GetWithItemsAsync(orderId) ?? throw new KeyNotFoundException("Order not found.");
            return order.Items.Sum(i => i.UnitPrice * i.Quantity);
        }

        public async Task<IReadOnlyList<Order>> GetUserOrdersAsync(string userId)
        {
            return await _orders.ListByUserAsync(userId);
        }
    }
}