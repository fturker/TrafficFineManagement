using TrafficFineManagement.Web.Data.Repositories.Interfaces;
using TrafficFineManagement.Web.Models.Entities;

namespace TrafficFineManagement.Web.Data.Repositories.Implementations
{
    public class PaymentRepository(ApplicationDbContext context) : IPaymentRepository
    {
        public void Add(Payment payment)
        {
            context.Payments.Add(payment);
        }

        public Task<int> SaveChangesAsync()
        {
            return context.SaveChangesAsync();
        }
    }
}
