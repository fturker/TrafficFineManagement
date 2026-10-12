using TrafficFineManagement.Web.Models.Entities;
using TrafficFineManagement.Web.Models.Enums;

namespace TrafficFineManagement.Web.Data.Repositories.Interfaces
{
    public interface ICarRepository
    {
        Task<List<Car>> GetAllAsync(
            int pageNumber,
            int pageSize,
            string? ownerId,
            string? searchTerm,
            CarType? carType
            );

        Task<Car?> GetByIdAsync(int id);

        Task<Car?> GetByPlateAsync(string plate);

        Task<bool> HasFinesAsync(int carId);

        void Add(Car car);

        void Delete(Car car);

        Task<int> SaveChangesAsync();
    }
}
