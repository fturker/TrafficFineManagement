using TrafficFineManagement.Web.Data.Repositories.Interfaces;
using TrafficFineManagement.Web.Models.Common;
using TrafficFineManagement.Web.Models.DTOs.Fine;
using TrafficFineManagement.Web.Models.Entities;
using TrafficFineManagement.Web.Models.Enums;
using TrafficFineManagement.Web.Services.Interfaces;

namespace TrafficFineManagement.Web.Services.Implementations
{
    public class FineService(
        IFineRepository fineRepository, 
        ICarRepository carRepository, 
        IUserRepository userRepository) : IFineService
    {
        public async Task<Result<FineDetailResponse>> CreateAsync(CreateFineRequest request, string actingUserId)
        {
            var car = await carRepository.GetByIdAsync(request.CarId);

            if (car is null)
            {
                return Result<FineDetailResponse>.Failure(
                    "Car not found.");
            }

            var user = await userRepository.GetByIdAsync(actingUserId);

            if (user is null)
            {
                return Result<FineDetailResponse>.Failure(
                    "Acting user not found.");
            }

            var fine = new Fine
            {
                CarId = car.Id,
                Car = car,

                Amount = request.Amount,
                Description = request.Description.Trim(),
                FineDate = request.FineDate,

                CreatedByUserId = user.Id,
                CreatedByUser = user,

                Status = FineStatus.New,
                IsPaid = false,
                PaidTime = null,
            };

            AddHistory(
                fine,
                user,
                FineActionType.Created,
                "Fine created.",
                previousStatus: null,
                createdAt: DateTime.UtcNow);

            fineRepository.Add(fine);

            await fineRepository.SaveChangesAsync();

            return Result<FineDetailResponse>.Success(
                MapToDetailResponse(fine));

        }

        public Task<Result<List<FineListResponse>>> GetAllAsync(FineFilterRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<FineDetailResponse>> GetByIdAsync(int id)
        {
            var fine = await fineRepository.GetByIdAsync(id);

            if (fine is null)
            {
                return Result<FineDetailResponse>.Failure(
                    "Fine not found.");
            }

            return Result<FineDetailResponse>.Success(
                MapToDetailResponse(fine));
        }

        public async Task<Result<FineDetailResponse>> UpdateAsync(int id, UpdateFineRequest request, string actingUserId)
        {
            var fine = await fineRepository.GetByIdAsync(id);

            if (fine is null)
            {
                return Result<FineDetailResponse>.Failure(
                    "Fine not found.");
            }

            var user = await userRepository.GetByIdAsync(actingUserId);

            if (user is null)
            {
                return Result<FineDetailResponse>.Failure(
                    "Acting user not found.");
            }

            var previousStatus = fine.Status;
            var now = DateTime.UtcNow;

            fine.Amount = request.Amount;
            fine.Description = request.Description.Trim();
            fine.FineDate = request.FineDate;
            fine.LastUpdatedAt = now;

            AddHistory(
                fine,
                user,
                FineActionType.Updated,
                "Fine details updated.",
                previousStatus,
                now);

            await fineRepository.SaveChangesAsync();

            return Result<FineDetailResponse>.Success(
                MapToDetailResponse(fine));
        }

        private void AddHistory(
            Fine fine,
            User user,
            FineActionType actionType,
            string description,
            FineStatus? previousStatus,
            DateTime createdAt)
        {
            fine.Histories.Add(new FineHistory
            {
                Fine = fine,

                UserId = user.Id,
                User = user,

                ActionType = actionType,
                Description = description,

                PreviousStatus = previousStatus,
                NewStatus = fine.Status,
                NextStatus = null,

                CreatedAt = createdAt
            });
        }

        private FineDetailResponse MapToDetailResponse(Fine fine)
        {
            return new FineDetailResponse
            {
                Id = fine.Id,

                CarId = fine.CarId,
                Plate = fine.Car.Plate,

                Amount = fine.Amount,
                Description = fine.Description,
                FineDate = fine.FineDate,

                Status = fine.Status,
                IsPaid = fine.IsPaid,
                PaidTime = fine.PaidTime,

                CreatedByUserId = fine.CreatedByUserId,
                CreatedByUserName =
                    $"{fine.CreatedByUser.Name} {fine.CreatedByUser.Surname}".Trim(),

                CreatedAt = fine.CreatedAt,
                LastUpdatedAt = fine.LastUpdatedAt
            };
        }
    }
}
