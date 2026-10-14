using TrafficFineManagement.Web.Data.Repositories.Implementations;
using TrafficFineManagement.Web.Data.Repositories.Interfaces;
using TrafficFineManagement.Web.Models.Common;
using TrafficFineManagement.Web.Models.DTOs.Car;
using TrafficFineManagement.Web.Models.Entities;
using TrafficFineManagement.Web.Models.Enums;
using TrafficFineManagement.Web.Rules;
using TrafficFineManagement.Web.Services.Interfaces;

namespace TrafficFineManagement.Web.Services.Implementations
{
    public class CarService(
        ICarRepository carRepository,
        IUserRepository userRepository) : ICarService
    {
        public async Task<Result<CarDetailResponse>> CreateAsync(CreateCarRequest request)
        {
            var owner = await userRepository.GetByIdAsync(request.OwnerId);

            if(owner is null)
            {
                return Result<CarDetailResponse>.Failure("Owner not found.");
            }

            var plate = NormalizePlate(request.Plate);

            var car = new Car
            {
                Plate = plate,
                CarType = request.CarType,
                Brand = request.Brand.Trim(),
                Model = request.Model.Trim(),
                OwnerId = request.OwnerId
            };

            carRepository.Add(car);
            await carRepository.SaveChangesAsync();

            return Result<CarDetailResponse>.Success(MapToDetailResponse(car));
        }

        public async Task<Result<bool>> DeleteAsync(int id)
        {
            var car = await carRepository.GetByIdAsync(id);

            if (car is null)
            {
                return Result<bool>.Failure("Car not found.");
            }

            carRepository.Delete(car);
            await carRepository.SaveChangesAsync();

            return Result<bool>.Success(true);

        }

        public async Task<Result<List<CarListResponse>>> GetAllAsync(CarFilterRequest request)
        {
            var pageValidate = PaginationRules.Validate(request.PageNumber, request.PageSize);

            if (!pageValidate.IsSuccess)
            {
                return Result<List<CarListResponse>>.Failure(pageValidate.Message);
            }

            var cars = await carRepository.GetAllAsync(
                pageNumber: request.PageNumber,
                pageSize: request.PageSize,
                ownerId: null,
                searchTerm: request.SearchTerm,
                carType: request.CarType);

            var response = cars.Select(car => new CarListResponse
            {
                Id = car.Id,
                Plate = car.Plate,
                CarType = car.CarType,
                Brand = car.Brand,
                Model = car.Model,
                OwnerDisplayName = $"{car.User.Name} {car.User.Surname}".Trim(),
                CompanyName = car.User.CompanyName
            }).ToList();

            return Result<List<CarListResponse>>.Success(response);
        }

        public async Task<Result<CarDetailResponse>> GetByIdAsync(int id)
        {
            var car = await carRepository.GetByIdAsync(id);

            if (car is null)
            {
                return Result<CarDetailResponse>.Failure(
                    "Car not found.");
            }

            return Result<CarDetailResponse>.Success(
                MapToDetailResponse(car));
        }

        public async Task<Result<CarDetailResponse>> GetByPlateAsync(string plate)
        {
            var normalizedPlate = NormalizePlate(plate);

            var car = await carRepository.GetByPlateAsync(normalizedPlate);

            if (car is null)
            {
                return Result<CarDetailResponse>.Failure(
                    "Car not found.");
            }

            return Result<CarDetailResponse>.Success(
                MapToDetailResponse(car));
        }

        public async Task<Result<CarDetailResponse>> UpdateAsync(int id, UpdateCarRequest request)
        {
            var car = await carRepository.GetByIdAsync(id);

            if (car is null)
            {
                return Result<CarDetailResponse>.Failure(
                    "Car not found.");
            }

            var owner = await userRepository.GetByIdAsync(request.OwnerId);

            if (owner is null)
            {
                return Result<CarDetailResponse>.Failure(
                    "Car owner not found.");
            }

            car.Plate = NormalizePlate(request.Plate);
            car.CarType = request.CarType;
            car.Brand = request.Brand.Trim();
            car.Model = request.Model.Trim();
            car.OwnerId = owner.Id;
            car.User = owner;
            car.LastUpdatedAt = DateTime.UtcNow;

            await carRepository.SaveChangesAsync();

            return Result<CarDetailResponse>.Success(
                MapToDetailResponse(car));
        }

        private string NormalizePlate(string plate)
        {
            return plate.Replace(" ", "").ToUpper();
        }

        private CarDetailResponse MapToDetailResponse(Car car)
        {
            return new CarDetailResponse
            {
                Id = car.Id,
                Plate = car.Plate,
                CarType = car.CarType,
                Brand = car.Brand,
                Model = car.Model,

                OwnerId = car.OwnerId,
                OwnerName = car.User.Name,
                OwnerSurname = car.User.Surname,
                CompanyName = car.User.CompanyName,
                TaxNumber = car.User.TaxNumber,

                CreatedAt = car.CreatedAt,
                LastUpdatedAt = car.LastUpdatedAt
            };
        }
    }
}
