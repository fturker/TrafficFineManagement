using Microsoft.AspNetCore.Identity;
using TrafficFineManagement.Web.Models.Common;
using TrafficFineManagement.Web.Models.DTOs.Auth;
using TrafficFineManagement.Web.Models.Entities;
using TrafficFineManagement.Web.Services.Interfaces;

namespace TrafficFineManagement.Web.Services.Implementations
{
    public class AuthService(SignInManager<User> signInManager) : IAuthService
    {
        public async Task<Result<bool>> LoginAsync(LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Identifier) ||
                string.IsNullOrEmpty(request.Password))
            {
                return Result<bool>.Failure(
                    "Kullanıcı adı ve parola zorunludur.");
            }

            var result = await signInManager.PasswordSignInAsync(
                userName: request.Identifier.Trim(),
                password: request.Password,
                isPersistent: false,
                lockoutOnFailure: true);

            if (!result.Succeeded)
            {
                return Result<bool>.Failure(
                    "Hatalı kullanıcı adı veya parola.");
            }

            return Result<bool>.Success(true);
        }

        public Task LogoutAsync()
        {
            return signInManager.SignOutAsync();
        }
    }
}
