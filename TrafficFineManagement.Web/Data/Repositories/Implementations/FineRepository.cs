using Microsoft.EntityFrameworkCore;
using TrafficFineManagement.Web.Data.Repositories.Interfaces;
using TrafficFineManagement.Web.Models.Entities;
using TrafficFineManagement.Web.Models.Enums;

namespace TrafficFineManagement.Web.Data.Repositories.Implementations
{
    public class FineRepository(ApplicationDbContext context) : IFineRepository
    {
        public void Add(Fine fine)
        {
            context.Fines.Add(fine);
        }

        public Task<List<Fine>> GetAllAsync(int pageNumber, int pageSize, string? searchTerm, int? carId, FineStatus? status, bool? isPaid)
        {
            throw new NotImplementedException();
        }

        public Task<Fine?> GetByIdAsync(int id)
        {
            return context.Fines
                .Include(fine => fine.Car)
                .Include(fine => fine.CreatedByUser)
                .FirstOrDefaultAsync(fine => fine.Id == id);
        }

        public Task<int> SaveChangesAsync()
        {
            return context.SaveChangesAsync();
        }

        private IQueryable<Fine> ApplyFilters()    
        {
            throw new NotImplementedException();
        }
    }
}
