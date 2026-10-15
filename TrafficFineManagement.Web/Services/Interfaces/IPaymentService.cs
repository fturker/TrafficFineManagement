using TrafficFineManagement.Web.Models.Common;

namespace TrafficFineManagement.Web.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<Result<bool>> PayAsync(int fineId, string userId);
    }
}
