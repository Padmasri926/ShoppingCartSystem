using EshoppingZone.Models;
using EshoppingZone.Repositories.Interfaces;
using EshoppingZone.Services.Interfaces;

namespace EshoppingZone.Services.Implementations
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _carts;
        private readonly IProductRepository _products;

        public CartService(ICartRepository carts, IProductRepository products)
        {
            _carts = carts;
            _products = products;
        }

        public async Task<Cart> GetOrCreateCartAsync(string userId)
        {
            var cart = await _carts.GetCartWithItemsAsync(userId);
            if (cart is null)
            {
                cart = new Cart { UserId = userId };
                await _carts.AddAsync(cart);
            }
            return cart;
        }

        public async Task<Cart> AddProductAsync(string userId, int productId, int quantity)
        {
            if (quantity <= 0) throw new ArgumentException("Quantity must be > 0.");
            var cart = await GetOrCreateCartAsync(userId);
            var product = await _products.GetByIdAsync(productId) ?? throw new KeyNotFoundException("Product not found.");

            var existing = cart.Items.FirstOrDefault(i => i.ProductId == productId);
            if (existing is null)
                cart.Items.Add(new CartItem { CartId = cart.CartId, ProductId = productId, Quantity = quantity });
            else
                existing.Quantity += quantity;

            _carts.Update(cart);
            await _carts.SaveChangesAsync();
            return await _carts.GetCartWithItemsAsync(userId) ?? cart;
        }

        public async Task<Cart> RemoveProductAsync(string userId, int productId, int quantity = int.MaxValue)
        {
            var cart = await GetOrCreateCartAsync(userId);
            var existing = cart.Items.FirstOrDefault(i => i.ProductId == productId);
            if (existing is null) return cart;

            if (quantity >= existing.Quantity) cart.Items.Remove(existing);
            else existing.Quantity -= quantity;

            _carts.Update(cart);
            await _carts.SaveChangesAsync();
            return await _carts.GetCartWithItemsAsync(userId) ?? cart;
        }

        public async Task<decimal> GetTotalAsync(string userId)
        {
            var cart = await _carts.GetCartWithItemsAsync(userId);
            if (cart is null) return 0m;
            return cart.Items.Sum(i => i.Quantity * i.Product.Price);
        }
    }
}