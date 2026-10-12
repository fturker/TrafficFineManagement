using TrafficFineManagement.Web.Models.Common;
using TrafficFineManagement.Web.Models.DTOs.User;

namespace TrafficFineManagement.Web.Services.Interfaces
{
    public interface IUserService
    {
        Task<Result<UserResponse>> CreateAsync(CreateUserRequest request);

        Task<Result<UserResponse>> UpdateAsync(
            string id,
            UpdateUserRequest request);

        Task<Result<UserResponse>> GetByIdAsync(string id);

        Task<Result<List<UserResponse>>> GetAllAsync(
            UserFilterRequest request);
    }
}
