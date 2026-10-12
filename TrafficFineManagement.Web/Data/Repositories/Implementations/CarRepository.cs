using Microsoft.EntityFrameworkCore;
using TrafficFineManagement.Web.Data.Repositories.Interfaces;
using TrafficFineManagement.Web.Models.Entities;
using TrafficFineManagement.Web.Models.Enums;

namespace TrafficFineManagement.Web.Data.Repositories.Implementations
{
    public class CarRepository : ICarRepository
    {
        private readonly ApplicationDbContext _context;

        public CarRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(Car car)
        {
            _context.Cars.Add(car);
        }

        public void Delete(Car car)
        {
            _context.Cars.Remove(car);
        }

        public Task<List<Car>> GetAllAsync(int pageNumber, int pageSize, string? ownerId, string? searchTerm, CarType? carType)
        {
            var query = _context.Cars.AsNoTracking();

            query = ApplyFilters(query, ownerId, searchTerm, carType);

            return query
                .Include(c => c.User)
                .OrderBy(c => c.LastUpdatedAt)
                .ThenBy(c => c.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public Task<Car?> GetByIdAsync(int id)
        {
            return _context.Cars
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public Task<Car?> GetByPlateAsync(string plate)
        {
            return _context.Cars
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.Plate == plate);
        }

        public Task<bool> HasFinesAsync(int carId)
        {
            return _context.Fines.AnyAsync(f => f.CarId == carId);
        }

        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }

        private IQueryable<Car> ApplyFilters(IQueryable<Car> query, string? ownerId, string? searchTerm, CarType? carType)
        {
            if (!string.IsNullOrEmpty(ownerId))
            {
                query = query.Where(c => c.OwnerId == ownerId);
            }
            if (!string.IsNullOrEmpty(searchTerm))
            {
                var trimmedSearchTerm = searchTerm!.Trim();

                query = query.Where(c => c.Plate.Contains(trimmedSearchTerm) || c.Brand.Contains(trimmedSearchTerm) || c.Model.Contains(trimmedSearchTerm));
            }
            if (carType.HasValue)
            {
                query = query.Where(c => c.CarType == carType.Value);
            }
            return query;
        }
    }
}
