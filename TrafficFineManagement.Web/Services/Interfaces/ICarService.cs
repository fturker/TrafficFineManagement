using TrafficFineManagement.Web.Models.Common;
using TrafficFineManagement.Web.Models.DTOs.Car;

namespace TrafficFineManagement.Web.Services.Interfaces
{
    public interface ICarService
    {
        Task<Result<CarDetailResponse>> CreateAsync(CreateCarRequest request);

        Task<Result<bool>> DeleteAsync(int id);

        Task<Result<CarDetailResponse>> UpdateAsync(int id, UpdateCarRequest request);

        Task<Result<CarDetailResponse>> GetByIdAsync(int id);

        Task<Result<List<CarListResponse>>> GetAllAsync(CarFilterRequest request);

        Task<Result<CarDetailResponse>> GetByPlateAsync(string plate);
    }
}
