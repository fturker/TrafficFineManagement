using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using TrafficFineManagement.Web.Consts;
using TrafficFineManagement.Web.Data;
using TrafficFineManagement.Web.Data.Repositories.Interfaces;
using TrafficFineManagement.Web.Models.Common;
using TrafficFineManagement.Web.Models.DTOs.User;
using TrafficFineManagement.Web.Models.Entities;
using TrafficFineManagement.Web.Models.Enums;
using TrafficFineManagement.Web.Rules.Interfaces;
using TrafficFineManagement.Web.Services.Interfaces;

namespace TrafficFineManagement.Web.Services.Implementations
{
    public class UserService(
        IUserRepository userRepository,
        UserManager<User> userManager,
        ApplicationDbContext context,
        IUserBusinessRules userBusinessRules) : IUserService
    {
        public async Task<Result<UserResponse>> CreateAsync(CreateUserRequest request)
        {
            var ruleResult = await userBusinessRules
                .ValidateUserCreationAsync(request);

            if (!ruleResult.IsSuccess)
            {
                return Result<UserResponse>.Failure(ruleResult.Message);
            }

            var user = CreateUserEntity(request);

            try
            {
                await using var transaction =
                    await context.Database.BeginTransactionAsync();

                var creationResult = await userManager.CreateAsync(
                    user,
                    request.Password);

                if (!creationResult.Succeeded)
                {
                    return Result<UserResponse>.Failure(
                        GetIdentityErrors(creationResult));
                }

                var roleResult = await userManager.AddToRoleAsync(
                    user,
                    request.Role);

                if (!roleResult.Succeeded)
                {
                    return Result<UserResponse>.Failure(
                        GetIdentityErrors(roleResult));
                }

                await transaction.CommitAsync();

                return Result<UserResponse>.Success(
                    MapToResponse(user));
            }
            catch (DbUpdateException exception)
                when (IsUniqueConstraintViolation(exception))
            {
                return Result<UserResponse>.Failure(
                    "The username, identity number or tax number is already in use.");
            }
        }

        public async Task<Result<List<UserResponse>>> GetAllAsync(UserFilterRequest request)
        {
            var ruleResult = userBusinessRules
                .ValidateUserListing(request);

            if (!ruleResult.IsSuccess)
            {
                return Result<List<UserResponse>>.Failure(
                    ruleResult.Message);
            }

            var users = await userRepository.GetAllAsync(
                pageNumber: request.PageNumber,
                pageSize: request.PageSize,
                searchTerm: request.SearchTerm,
                accountType: request.AccountType);

            var response = users
                .Select(MapToResponse)
                .ToList();

            return Result<List<UserResponse>>.Success(response);
        }

        public async Task<Result<UserResponse>> GetByIdAsync(string id)
        {
            var ruleResult = userBusinessRules
                .ValidateUserManagementAccess();

            if (!ruleResult.IsSuccess)
            {
                return Result<UserResponse>.Failure(ruleResult.Message);
            }

            var user = await userRepository.GetByIdAsync(id);

            if (user is null)
            {
                return Result<UserResponse>.Failure("User not found.");
            }

            return Result<UserResponse>.Success(
                MapToResponse(user));
        }

        public async Task<Result<UserResponse>> UpdateAsync(string id, UpdateUserRequest request)
        {
            var accessResult = userBusinessRules
                .ValidateUserManagementAccess();

            if (!accessResult.IsSuccess)
            {
                return Result<UserResponse>.Failure(accessResult.Message);
            }

            var user = await userRepository.GetByIdAsync(id);

            if (user is null)
            {
                return Result<UserResponse>.Failure("User not found.");
            }

            var ruleResult = await userBusinessRules
                .ValidateUserUpdateAsync(user, request);

            if (!ruleResult.IsSuccess)
            {
                return Result<UserResponse>.Failure(ruleResult.Message);
            }

            user.Name = request.Name.Trim();
            user.Surname = request.Surname.Trim();
            user.Address = request.Address?.Trim();
            user.BirthDate = request.BirthDate;

            user.CompanyName = user.AccountType == AccountType.Business
                ? request.CompanyName!.Trim()
                : null;

            user.LastUpdatedAt = DateTime.UtcNow;

            var updateResult = await userManager.UpdateAsync(user);

            if (!updateResult.Succeeded)
            {
                return Result<UserResponse>.Failure(
                    GetIdentityErrors(updateResult));
            }

            return Result<UserResponse>.Success(
                MapToResponse(user));
        }

        private User CreateUserEntity(CreateUserRequest request)
        {
            var isCitizen = request.Role == RoleName.Citizen;
            var isBusiness = request.AccountType == AccountType.Business;

            var now = DateTime.UtcNow;

            return new User
            {
                UserName = GetUserName(request),

                Name = request.Name.Trim(),
                Surname = request.Surname.Trim(),

                AccountType = request.AccountType,

                IdentityNumber = isCitizen && !isBusiness
                    ? request.IdentityNumber
                    : null,

                TaxNumber = isBusiness
                    ? request.TaxNumber
                    : null,

                CompanyName = isBusiness
                    ? request.CompanyName!.Trim()
                    : null,

                Address = request.Address?.Trim(),
                BirthDate = request.BirthDate,

                CreatedAt = now,
                LastUpdatedAt = now
            };
        }
        private string GetUserName(CreateUserRequest request)
        {
            if (request.Role != RoleName.Citizen)
            {
                return request.UserName!.Trim();
            }

            return request.AccountType == AccountType.Business
                ? request.TaxNumber!
                : request.IdentityNumber!;
        }

        private string GetIdentityErrors(IdentityResult result)
        {
            return string.Join(
                " ",
                result.Errors.Select(error => error.Description));
        }

        private bool IsUniqueConstraintViolation(
            DbUpdateException exception)
        {
            return exception.InnerException is SqlException
            {
                Number: 2601 or 2627
            };
        }

        private UserResponse MapToResponse(User user)
        {
            return new UserResponse
            {
                Id = user.Id,
                UserName = user.UserName ?? string.Empty,

                Name = user.Name,
                Surname = user.Surname,

                AccountType = user.AccountType,

                IdentityNumber = user.IdentityNumber,
                TaxNumber = user.TaxNumber,
                CompanyName = user.CompanyName,

                Address = user.Address,
                BirthDate = user.BirthDate,

                CreatedAt = user.CreatedAt,
                LastUpdatedAt = user.LastUpdatedAt
            };
        }
    }
}
