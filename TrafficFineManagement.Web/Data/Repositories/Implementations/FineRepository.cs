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

        public Task<List<Fine>> GetAllAsync(int pageNumber, int pageSize, string? searchTerm, int? carId, FineStatus? status, bool? isPaid, string? ownerId = null, string? createdByUserId = null)
        {
            var query = context.Fines.AsNoTracking();

            query = ApplyFilters(query, searchTerm, carId, status, isPaid, ownerId, createdByUserId);
            return query
                .Include(fine => fine.Car)
                .Include(fine => fine.CreatedByUser)
                .Include(fine => fine.Histories)
                .ThenInclude(history => history.User)
                .OrderByDescending(fine => fine.CreatedAt)
                .ThenByDescending(fine => fine.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public Task<Fine?> GetByIdAsync(int id)
        {
            return context.Fines
                .Include(fine => fine.Car)
                .Include(fine => fine.CreatedByUser)
                .Include(fine => fine.Histories)
                .ThenInclude(history => history.User)
                .FirstOrDefaultAsync(fine => fine.Id == id);
        }

        public Task<int> SaveChangesAsync()
        {
            return context.SaveChangesAsync();
        }

        private IQueryable<Fine> ApplyFilters(
            IQueryable<Fine> query,
            string? searchTerm,
            int? carId,
            FineStatus? status,
            bool? isPaid,
            string? ownerId = null,
            string? createdByUserId = null)
        {
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var search = searchTerm.Trim();

                query = query.Where(fine =>
                    fine.Car.Plate.Contains(search) ||
                    fine.Description.Contains(search));
            }

            if (carId.HasValue)
            {
                query = query.Where(fine => fine.CarId == carId.Value);
            }

            if (status.HasValue)
            {
                query = query.Where(fine => fine.Status == status.Value);
            }

            if (isPaid.HasValue)
            {
                query = query.Where(fine => fine.IsPaid == isPaid.Value);
            }

            if(!string.IsNullOrWhiteSpace(ownerId))
            {
                query = query.Where(fine => fine.Car.OwnerId == ownerId);
            }

            if (!string.IsNullOrWhiteSpace(createdByUserId))
            {
                query = query.Where(fine =>
                    fine.CreatedByUserId == createdByUserId);
            }

            return query;
        }
    }
}
