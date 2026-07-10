using EshoppingZone.Models;
using EshoppingZone.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EshoppingZone.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _products;
        public ProductsController(IProductRepository products) => _products = products;

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _products.ListAsync());

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var p = await _products.GetByIdAsync(id);
            return p is null ? NotFound() : Ok(p);
        }

        [Authorize(Roles = "Merchant")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Product p)
        {
            var created = await _products.AddAsync(p);
            return CreatedAtAction(nameof(Get), new { id = created.ProductId }, created);
        }

        [Authorize(Roles = "Merchant")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] Product p)
        {
            var existing = await _products.GetByIdAsync(id);
            if (existing is null) return NotFound();

            existing.ProductName = p.ProductName;
            existing.Description = p.Description;
            existing.Category = p.Category;
            existing.Price = p.Price;
            existing.Stock = p.Stock;

            _products.Update(existing);
            await _products.SaveChangesAsync();
            return Ok(existing);
        }

        [Authorize(Roles = "Merchant")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _products.GetByIdAsync(id);
            if (existing is null) return NotFound();
            _products.Delete(existing);
            await _products.SaveChangesAsync();
            return NoContent();
        }
    }
}