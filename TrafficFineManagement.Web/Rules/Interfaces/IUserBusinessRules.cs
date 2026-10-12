using TrafficFineManagement.Web.Models.Common;
using TrafficFineManagement.Web.Models.DTOs.User;
using TrafficFineManagement.Web.Models.Entities;

namespace TrafficFineManagement.Web.Rules.Interfaces
{
    public interface IUserBusinessRules
    {
        Result<bool> ValidateUserManagementAccess();

        Task<Result<bool>> ValidateUserCreationAsync(
            CreateUserRequest request);

        Task<Result<bool>> ValidateUserUpdateAsync(
            User user,
            UpdateUserRequest request);

        Result<bool> ValidateUserListing(
            UserFilterRequest request);
    }
}
