using TrafficFineManagement.Web.Models.Entities;

namespace TrafficFineManagement.Web.Data.Repositories.Interfaces
{
    public interface IFineHistoryRepository
    {
        Task<List<FineHistory>> GetByFineIdAsync(int fineId);
    }
}
