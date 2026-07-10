using EshoppingZone.Dtos;
using EshoppingZone.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EshoppingZone.Controllers
{
    [ApiController]
    [Route("api/orders")]
    [Authorize(Roles = "Customer,Admin")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orders;
        public OrdersController(IOrderService orders) => _orders = orders;

        [HttpPost("place")]
        public async Task<IActionResult> Place([FromBody] PlaceOrderRequest request)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
                var order = await _orders.PlaceOrderAsync(userId, request.PaymentMode);
                var total = await _orders.CalculateTotalAsync(order.OrderId);
                return Ok(new PlaceOrderResponse(order.OrderId, total, order.Status));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{orderId:int}")]
        public async Task<IActionResult> Get(int orderId)
        {
            var order = await _orders.GetOrderAsync(orderId);
            return order is null ? NotFound() : Ok(order);
        }

        [HttpGet("my-orders")]
        public async Task<IActionResult> GetMyOrders()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var orders = await _orders.GetUserOrdersAsync(userId);
            return Ok(orders);
        }
    }

    public record PlaceOrderRequest(string PaymentMode);
}