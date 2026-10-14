using TrafficFineManagement.Web.Models.Common;
using TrafficFineManagement.Web.Models.Entities;
using TrafficFineManagement.Web.Models.Enums;

namespace TrafficFineManagement.Web.Rules.Interfaces
{
    public interface IFineBusinessRules
    {
        Task<Result<bool>> ValidateDecisionAsync(
            Fine fine,
            User user,
            FineStatus? expectedStatus);

        Result<bool> ValidateRejectionReason(string? reason);

        Task<Result<bool>> ValidateUpdateAsync(Fine fine, User user);
    }
}
