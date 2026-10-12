using TrafficFineManagement.Web.Models.Entities;
using TrafficFineManagement.Web.Models.Enums;

namespace TrafficFineManagement.Web.Data.Repositories.Interfaces
{
    public interface IFineRepository
    {
        Task<List<Fine>> GetAllAsync(
            int pageNumber,
            int pageSize,
            string? searchTerm,
            int? carId,
            FineStatus? status,
            bool? isPaid);

        Task<Fine?> GetByIdAsync(int id);

        void Add(Fine fine);

        Task<int> SaveChangesAsync();
    }
}
