using TrafficFineManagement.Web.Models.Common;
using TrafficFineManagement.Web.Models.DTOs.Fine;

namespace TrafficFineManagement.Web.Services.Interfaces
{
    public interface IFineService
    {
        Task<Result<FineDetailResponse>> CreateAsync(
            CreateFineRequest request,
            string actingUserId);

        Task<Result<FineDetailResponse>> UpdateAsync(
            int id,
            UpdateFineRequest request,
            string actingUserId);

        Task<Result<FineDetailResponse>> GetByIdAsync(int id);

        Task<Result<List<FineListResponse>>> GetAllAsync(
            FineFilterRequest request);
    }
}
