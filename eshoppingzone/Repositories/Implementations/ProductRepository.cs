using EshoppingZone.Data;
using EshoppingZone.Models;
using EshoppingZone.Repositories.Interfaces;

namespace EshoppingZone.Repositories.Implementations
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext db) : base(db) { }
    }
}