using EshoppingZone.Data;
using EshoppingZone.Models;
using EshoppingZone.Repositories.Interfaces;

namespace EshoppingZone.Repositories.Implementations
{
    public class PaymentRepository : GenericRepository<Payment>, IPaymentRepository
    {
        public PaymentRepository(AppDbContext db) : base(db) { }
    }
}