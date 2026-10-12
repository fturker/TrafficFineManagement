using TrafficFineManagement.Web.Models.Common;
using TrafficFineManagement.Web.Models.DTOs.Auth;

namespace TrafficFineManagement.Web.Services.Interfaces
{
    public interface IAuthService
    {
        Task<Result<bool>> LoginAsync(LoginRequest request);

        Task LogoutAsync();
    }
}
