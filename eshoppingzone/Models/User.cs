using Microsoft.AspNetCore.Identity;

namespace EshoppingZone.Models
{
    public class User : IdentityUser
    {
        public string Role { get; set; } = "Customer"; // Admin, Merchant, Customer
        public bool IsApproved { get; set; } = true; // Merchants need approval

        // Navigation
        public Cart? Cart { get; set; }
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public Wallet? Wallet { get; set; }
    }
}