using EshoppingZone.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EshoppingZone.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class WalletController : ControllerBase
    {
        private readonly IWalletService _walletService;

        public WalletController(IWalletService walletService)
        {
            _walletService = walletService;
        }

        [HttpGet("balance")]
        public async Task<IActionResult> GetBalance()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var balance = await _walletService.GetBalanceAsync(userId);
            return Ok(new { balance });
        }

        [HttpPost("topup")]
        public async Task<IActionResult> TopUp([FromBody] TopUpDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            await _walletService.TopUpAsync(userId, dto.Amount);
            var balance = await _walletService.GetBalanceAsync(userId);
            return Ok(new { message = "Top-up successful", balance });
        }
    }

    public record TopUpDto(decimal Amount);
}
