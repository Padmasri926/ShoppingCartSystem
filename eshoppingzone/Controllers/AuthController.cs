using EshoppingZone.Models;
using EshoppingZone.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EshoppingZone.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IJwtService _jwtService;

        public AuthController(UserManager<User> userManager, IJwtService jwtService)
        {
            _userManager = userManager;
            _jwtService = jwtService;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            // Validate email format
            if (!IsValidEmail(dto.Email))
                return BadRequest(new { message = "Invalid email format" });

            // Validate role
            var validRoles = new[] { "Customer", "Merchant", "Admin" };
            if (!validRoles.Contains(dto.Role))
                return BadRequest(new { message = "Invalid role. Must be Customer, Merchant, or Admin" });

            // Check if admin already exists
            if (dto.Role == "Admin")
            {
                var existingAdmin = _userManager.Users.FirstOrDefault(u => u.Role == "Admin");
                if (existingAdmin != null)
                    return BadRequest(new { message = "Admin already exists. Only one admin is allowed." });
            }

            var user = new User
            {
                UserName = dto.Email,
                Email = dto.Email,
                Role = dto.Role,
                IsApproved = dto.Role != "Merchant" // Merchants need approval, Admin and Customer are auto-approved
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok(new { message = dto.Role == "Merchant" ? "Registration successful. Awaiting admin approval." : "Registration successful." });
        }

        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
                return Unauthorized(new { message = "Invalid credentials" });

            if (!user.IsApproved)
                return Unauthorized(new { message = "Account pending approval" });

            var token = _jwtService.GenerateToken(user);
            return Ok(new { token, role = user.Role });
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("merchants/pending")]
        public async Task<IActionResult> GetPendingMerchants()
        {
            var merchants = _userManager.Users.Where(u => u.Role == "Merchant" && !u.IsApproved).ToList();
            return Ok(merchants.Select(m => new { m.Id, m.Email, m.IsApproved }));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("merchants/{id}/approve")]
        public async Task<IActionResult> ApproveMerchant(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null || user.Role != "Merchant")
                return NotFound();

            user.IsApproved = true;
            await _userManager.UpdateAsync(user);
            return Ok(new { message = "Merchant approved" });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("merchants/{id}/reject")]
        public async Task<IActionResult> RejectMerchant(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null || user.Role != "Merchant")
                return NotFound();

            await _userManager.DeleteAsync(user);
            return Ok(new { message = "Merchant rejected" });
        }
    }

    public record RegisterDto(string Email, string Password, string Role);
    public record LoginDto(string Email, string Password);
}
