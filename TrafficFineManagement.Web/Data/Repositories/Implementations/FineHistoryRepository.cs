using Microsoft.EntityFrameworkCore;
using TrafficFineManagement.Web.Data.Repositories.Interfaces;
using TrafficFineManagement.Web.Models.Entities;

namespace TrafficFineManagement.Web.Data.Repositories.Implementations
{
    public class FineHistoryRepository(ApplicationDbContext context) : IFineHistoryRepository
    {
        public Task<List<FineHistory>> GetByFineIdAsync(int fineId)
        {
            return context.FineHistories
                .AsNoTracking()
                .Include(history => history.User)
                .Where(history => history.FineId == fineId)
                .OrderBy(history => history.CreatedAt)
                .ThenBy(history => history.Id)
                .ToListAsync();
        }
    }
}
