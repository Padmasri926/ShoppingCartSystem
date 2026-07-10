using EshoppingZone.Dtos;
using EshoppingZone.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EshoppingZone.Controllers
{
    [ApiController]
    [Route("api/cart")]
    [Authorize(Roles = "Customer,Admin")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _carts;
        public CartController(ICartService carts) => _carts = carts;

        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var cart = await _carts.GetOrCreateCartAsync(userId);
            var total = await _carts.GetTotalAsync(userId);
            return Ok(new
            {
                cart.CartId,
                cart.UserId,
                items = cart.Items.Select(i => new { i.ProductId, i.Product.ProductName, i.Quantity, i.Product.Price }),
                total
            });
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddItem([FromBody] AddProductToCartRequest dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            await _carts.AddProductAsync(userId, dto.ProductId, dto.Quantity);
            return Ok(new { message = "Product added to cart successfully" });
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateItem([FromBody] AddProductToCartRequest dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            await _carts.AddProductAsync(userId, dto.ProductId, dto.Quantity);
            return Ok(new { message = "Cart updated successfully" });
        }

        [HttpDelete("remove/{productId:int}")]
        public async Task<IActionResult> RemoveItem(int productId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            await _carts.RemoveProductAsync(userId, productId, int.MaxValue);
            return Ok(new { message = "Product removed from cart successfully" });
        }

        [HttpGet("total")]
        public async Task<IActionResult> GetTotal()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            return Ok(await _carts.GetTotalAsync(userId));
        }
    }
}