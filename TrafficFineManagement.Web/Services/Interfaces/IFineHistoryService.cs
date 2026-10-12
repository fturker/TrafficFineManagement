using TrafficFineManagement.Web.Models.Common;
using TrafficFineManagement.Web.Models.DTOs.FineHistory;

namespace TrafficFineManagement.Web.Services.Interfaces
{
    public interface IFineHistoryService
    {
        Task<Result<List<FineHistoryResponse>>> GetByFineIdAsync(
            int fineId);
    }
}
