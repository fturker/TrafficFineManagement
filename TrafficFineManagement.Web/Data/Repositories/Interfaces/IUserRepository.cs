using TrafficFineManagement.Web.Models.Entities;
using TrafficFineManagement.Web.Models.Enums;

namespace TrafficFineManagement.Web.Data.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(string id);

        Task<List<User>> GetAllAsync(
            int pageNumber,
            int pageSize,
            string? searchTerm,
            AccountType? accountType);
    }
}
