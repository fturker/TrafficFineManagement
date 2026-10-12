using TrafficFineManagement.Web.Data.Repositories.Interfaces;
using TrafficFineManagement.Web.Models.Common;
using TrafficFineManagement.Web.Models.DTOs.FineHistory;
using TrafficFineManagement.Web.Models.Entities;
using TrafficFineManagement.Web.Services.Interfaces;

namespace TrafficFineManagement.Web.Services.Implementations
{
    public class FineHistoryService(IFineHistoryRepository fineHistoryRepository,
        ) : IFineHistoryService
    {
        public async Task<Result<List<FineHistoryResponse>>> GetByFineIdAsync(int fineId)
        {
            if (fineId <= 0)
            {
                return Result<List<FineHistoryResponse>>.Failure(
                    "Invalid fine ID.");
            }

            var histories = await fineHistoryRepository
                .GetByFineIdAsync(fineId);

            var response = histories
                .Select(MapToResponse)
                .ToList();

            return Result<List<FineHistoryResponse>>.Success(response);
        }

        private FineHistoryResponse MapToResponse(
            FineHistory history)
        {
            return new FineHistoryResponse
            {
                Id = history.Id,
                FineId = history.FineId,

                UserId = history.UserId,
                UserDisplayName =
                    $"{history.User.Name} {history.User.Surname}".Trim(),

                ActionType = history.ActionType,
                Description = history.Description,

                PreviousStatus = history.PreviousStatus,
                NewStatus = history.NewStatus,

                CreatedAt = history.CreatedAt
            };
        }
    }
}
