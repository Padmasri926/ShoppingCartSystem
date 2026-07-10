using EshoppingZone.Dtos;
using EshoppingZone.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EshoppingZone.Controllers
{
    [ApiController]
    [Route("api/payments")]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _payments;
        private readonly IOrderService _orders;

        public PaymentsController(IPaymentService payments, IOrderService orders)
        {
            _payments = payments;
            _orders = orders;
        }

        [HttpPost]
        public async Task<IActionResult> MakePayment([FromBody] MakePaymentRequest dto)
        {
            var payment = await _payments.MakePaymentAsync(dto.OrderId, dto.PaymentMode, dto.Amount, dto.WalletId);
            var total = await _orders.CalculateTotalAsync(dto.OrderId);
            return Ok(new
            {
                payment.PaymentId,
                payment.PaymentMode,
                total,
                transactions = payment.Transactions.Select(t => new { t.PaymentTransactionId, t.Amount, t.TransactionDate })
            });
        }
    }
}