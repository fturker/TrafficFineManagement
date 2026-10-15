using Microsoft.EntityFrameworkCore;
using TrafficFineManagement.Web.Data.Repositories.Interfaces;
using TrafficFineManagement.Web.Models.Common;
using TrafficFineManagement.Web.Models.DTOs.Car;
using TrafficFineManagement.Web.Models.DTOs.Fine;
using TrafficFineManagement.Web.Models.Entities;
using TrafficFineManagement.Web.Models.Enums;
using TrafficFineManagement.Web.Rules;
using TrafficFineManagement.Web.Rules.Interfaces;
using TrafficFineManagement.Web.Services.Interfaces;

namespace TrafficFineManagement.Web.Services.Implementations
{
    public class FineService(
        IFineRepository fineRepository,
        ICarRepository carRepository,
        IUserRepository userRepository,
        IFineBusinessRules fineBusinessRules) : IFineService
    {
        public Task<Result<bool>> ApproveAsync(int id, FineDecisionRequest request, string actingUserId)
        {
            return ProcessDecisionAsync(id, request, actingUserId, FineActionType.Approved);
        }

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
                request.Description,
                previousStatus: null,
                createdAt: DateTime.UtcNow);

            fineRepository.Add(fine);

            await fineRepository.SaveChangesAsync();

            return Result<FineDetailResponse>.Success(
                MapToDetailResponse(fine));

        }

        public async Task<Result<List<FineListResponse>>> GetAllAsync(FineFilterRequest request)
        {
            var pageValidate = PaginationRules.Validate(request.PageNumber, request.PageSize);

            if (!pageValidate.IsSuccess)
            {
                return Result<List<FineListResponse>>.Failure(pageValidate.Message);
            }

            var fines = await fineRepository.GetAllAsync(
                pageNumber: request.PageNumber,
                pageSize: request.PageSize,
                searchTerm: request.SearchTerm,
                carId: request.CarId,
                status: request.Status,
                isPaid: request.IsPaid);

            var response = fines.Select(fine => new FineListResponse
            {
                Id = fine.Id,
                Plate = fine.Car.Plate,
                Amount = fine.Amount,
                FineDate = fine.FineDate,
                Status = fine.Status,
                IsPaid = fine.IsPaid
            }).ToList();

            return Result<List<FineListResponse>>.Success(response);
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

        public Task<Result<bool>> RejectAsync(int id, FineDecisionRequest request, string actingUserId)
        {
            return ProcessDecisionAsync(id, request, actingUserId, FineActionType.Rejected);
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

            var ruleResult = await fineBusinessRules.ValidateUpdateAsync(fine, user);

            if (!ruleResult.IsSuccess)
            {
                return Result<FineDetailResponse>.Failure(ruleResult.Message);
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

        public async Task<Result<List<FineListResponse>>> GetByOwnerId(FineFilterRequest request, string ownerId)
        {
            var pageValidate = PaginationRules.Validate(request.PageNumber, request.PageSize);

            if (!pageValidate.IsSuccess)
            {
                return Result<List<FineListResponse>>.Failure(pageValidate.Message);
            }

            var fines = await fineRepository.GetAllAsync(
                pageNumber: request.PageNumber,
                pageSize: request.PageSize,
                searchTerm: request.SearchTerm,
                carId: request.CarId,
                status: FineStatus.Completed,
                isPaid: request.IsPaid,
                ownerId: ownerId);

            var response = fines.Select(fine => new FineListResponse
            {
                Id = fine.Id,
                Plate = fine.Car.Plate,
                Amount = fine.Amount,
                FineDate = fine.FineDate,
                Status = fine.Status,
                IsPaid = fine.IsPaid
            }).ToList();

            return Result<List<FineListResponse>>.Success(response);
        }

        public async Task<Result<List<FineListResponse>>> GetByCreatedByUserId(FineFilterRequest request, string createdByUserId)
        {
            var pageValidate = PaginationRules.Validate(request.PageNumber, request.PageSize);

            if (!pageValidate.IsSuccess)
            {
                return Result<List<FineListResponse>>.Failure(pageValidate.Message);
            }

            var fines = await fineRepository.GetAllAsync(
                pageNumber: request.PageNumber,
                pageSize: request.PageSize,
                searchTerm: request.SearchTerm,
                carId: request.CarId,
                status: request.Status,
                isPaid: request.IsPaid,
                createdByUserId: createdByUserId);
            var response = fines.Select(fine => new FineListResponse
            {
                Id = fine.Id,
                Plate = fine.Car.Plate,
                Amount = fine.Amount,
                FineDate = fine.FineDate,
                Status = fine.Status,
                IsPaid = fine.IsPaid
            }).ToList();

            return Result<List<FineListResponse>>.Success(response);
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
                Description = description.Trim(),

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
                LastUpdatedAt = fine.LastUpdatedAt,

                Histories = fine.Histories
                    .OrderBy(history => history.CreatedAt)
                    .ThenBy(history => history.Id)
                    .Select(history => new FineHistoryItemResponse
                    {
                        CreatedAt = history.CreatedAt,

                        UserDisplayName = $"{history.User.Name} {history.User.Surname}".Trim(),

                        ActionType = history.ActionType,
                        Description = history.Description,

                        PreviousStatus = history.PreviousStatus,
                        NewStatus = history.NewStatus
                    }).ToList()
            };
        }

        private async Task<Result<bool>> ProcessDecisionAsync(
            int id,
            FineDecisionRequest request,
            string actingUserId,
            FineActionType actionType)
        {
            var fine = await fineRepository.GetByIdAsync(id);

            if (fine is null)
                return Result<bool>.Failure("Fine not found.");

            var user = await userRepository.GetByIdAsync(actingUserId);

            if (user is null)
                return Result<bool>.Failure("Acting user not found.");

            var accessResult = await fineBusinessRules.ValidateDecisionAsync(
                fine,
                user,
                request.ExpectedStatus);

            if (!accessResult.IsSuccess)
                return accessResult;

            if (actionType == FineActionType.Rejected)
            {
                var reasonResult = fineBusinessRules
                    .ValidateRejectionReason(request.Description);

                if (!reasonResult.IsSuccess)
                    return reasonResult;
            }

            var previousStatus = fine.Status;
            var now = DateTime.UtcNow;

            fine.Status = GetNextStatus(fine.Status, actionType);

            fine.LastUpdatedAt = now;

            var description = actionType == FineActionType.Rejected
                ? request.Description!.Trim()
                : string.IsNullOrWhiteSpace(request.Description) ? "Fine approved." : request.Description.Trim();

            AddHistory(
                fine,
                user,
                actionType,
                description,
                previousStatus,
                now);

            try
            {
                await fineRepository.SaveChangesAsync();

                return Result<bool>.Success(true);
            }
            catch (DbUpdateConcurrencyException)
            {
                return Result<bool>.Failure(
                    "The fine was changed by another user. Refresh the page.");
            }
        }

        private FineStatus GetNextStatus(FineStatus currentStatus, FineActionType actionType)
        {
            return (currentStatus, actionType) switch
            {
                (FineStatus.New, FineActionType.Approved)
                    => FineStatus.ManagerApproved,

                (FineStatus.ManagerApproved, FineActionType.Approved)
                    => FineStatus.Completed,

                (FineStatus.New, FineActionType.Rejected)
                    => FineStatus.Rejected,

                (FineStatus.ManagerApproved, FineActionType.Rejected)
                    => FineStatus.Rejected,

                _ => throw new InvalidOperationException(
                    "Geçersiz ceza durumu geçişi.")
            };
        }
    }
}
