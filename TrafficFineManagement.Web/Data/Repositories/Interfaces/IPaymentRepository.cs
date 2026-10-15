using TrafficFineManagement.Web.Models.Entities;

namespace TrafficFineManagement.Web.Data.Repositories.Interfaces
{
    public interface IPaymentRepository
    {
        void Add(Payment payment);

        Task<int> SaveChangesAsync();
    }
}
