using Microsoft.AspNetCore.Identity;
using TrafficFineManagement.Web.Consts;
using TrafficFineManagement.Web.Models.Common;
using TrafficFineManagement.Web.Models.DTOs.User;
using TrafficFineManagement.Web.Models.Entities;
using TrafficFineManagement.Web.Models.Enums;
using TrafficFineManagement.Web.Rules.Interfaces;

namespace TrafficFineManagement.Web.Rules.Implementations
{
    public class UserBusinessRules(
        UserManager<User> userManager,
        RoleManager<IdentityRole> roleManager,
        IHttpContextAccessor httpContextAccessor) : IUserBusinessRules

    {
        public async Task<Result<bool>> ValidateUserCreationAsync(CreateUserRequest request)
        {
            var result = ValidateUserManagementAccess();

            if (!result.IsSuccess)
                return result;

            result = ValidateRequest(request);

            if (!result.IsSuccess)
                return result;

            result = ValidateSupportedRole(request.Role);

            if (!result.IsSuccess)
                return result;

            result = ValidateRoleAssignmentPermission(request.Role);

            if (!result.IsSuccess)
                return result;

            result = await ValidateRoleExistsAsync(request.Role);

            if (!result.IsSuccess)
                return result;

            return ValidateAccountDetails(request);
        }

        public Result<bool> ValidateUserListing(UserFilterRequest request)
        {
            var result = ValidateUserManagementAccess();

            if (!result.IsSuccess)
                return result;

            result = ValidateRequest(request);

            if (!result.IsSuccess)
                return result;

            return PaginationRules.Validate(
                request.PageNumber,
                request.PageSize);
        }

        public Result<bool> ValidateUserManagementAccess()
        {
            if (IsCurrentUserInRole(RoleName.Admin) ||
                IsCurrentUserInRole(RoleName.SuperAdmin))
            {
                return Result<bool>.Success(true);
            }

            return Result<bool>.Failure("Access denied.");
        }

        public async Task<Result<bool>> ValidateUserUpdateAsync(User user, UpdateUserRequest request)
        {
            var result = ValidateUserManagementAccess();

            if (!result.IsSuccess)
                return result;

            result = ValidateRequest(request);

            if (!result.IsSuccess)
                return result;

            result = await ValidateTargetUserUpdatePermissionAsync(user);

            if (!result.IsSuccess)
                return result;

            return ValidateCompanyName(
                user.AccountType,
                request.CompanyName);
        }

        private static Result<bool> ValidateRequest(object request)
        {
            var error = RequestValidationRules.Validate(request);

            return error is null
                ? Result<bool>.Success(true)
                : Result<bool>.Failure(error);
        }

        private static Result<bool> ValidateSupportedRole(string role)
        {
            var isSupported = role is
                RoleName.Citizen or
                RoleName.Officer or
                RoleName.Admin or
                RoleName.Finance or
                RoleName.SuperAdmin;

            return isSupported
                ? Result<bool>.Success(true)
                : Result<bool>.Failure("Invalid role.");
        }

        private Result<bool> ValidateRoleAssignmentPermission(string role)
        {
            if (role == RoleName.Citizen ||
                IsCurrentUserInRole(RoleName.SuperAdmin))
            {
                return Result<bool>.Success(true);
            }

            return Result<bool>.Failure(
                "Only a super administrator can assign staff roles.");
        }

        private async Task<Result<bool>> ValidateRoleExistsAsync(string role)
        {
            var exists = await roleManager.RoleExistsAsync(role);

            return exists
                ? Result<bool>.Success(true)
                : Result<bool>.Failure(
                    "The selected role has not been configured.");
        }

        private Result<bool> ValidateAccountDetails(
            CreateUserRequest request)
        {
            if (request.Role != RoleName.Citizen)
            {
                return ValidateStaffAccount(
                    request.AccountType,
                    request.UserName);
            }

            if (request.AccountType == AccountType.Business)
            {
                return ValidateBusinessAccount(
                    request.TaxNumber,
                    request.CompanyName);
            }

            return ValidateIdentityNumberRequired(request.IdentityNumber);
        }

        private Result<bool> ValidateStaffAccount(
            AccountType accountType,
            string? userName)
        {
            if (accountType != AccountType.Individual)
            {
                return Result<bool>.Failure(
                    "Staff accounts must use the Individual account type.");
            }

            return ValidateStaffUserName(userName);
        }

        private Result<bool> ValidateStaffUserName(string? userName)
        {
            if (string.IsNullOrWhiteSpace(userName) ||
                !userName.Any(char.IsLetter))
            {
                return Result<bool>.Failure(
                    "Staff usernames must contain at least one letter.");
            }

            return Result<bool>.Success(true);
        }

        private Result<bool> ValidateBusinessAccount(
            string? taxNumber,
            string? companyName)
        {
            if (string.IsNullOrWhiteSpace(taxNumber))
            {
                return Result<bool>.Failure("Tax number is required.");
            }

            return ValidateCompanyName(AccountType.Business, companyName);
        }

        private Result<bool> ValidateIdentityNumberRequired(
            string? identityNumber)
        {
            if (string.IsNullOrWhiteSpace(identityNumber))
            {
                return Result<bool>.Failure(
                    "Identity number is required.");
            }

            return Result<bool>.Success(true);
        }

        private static Result<bool> ValidateCompanyName(
            AccountType accountType,
            string? companyName)
        {
            if (accountType == AccountType.Business &&
                string.IsNullOrWhiteSpace(companyName))
            {
                return Result<bool>.Failure(
                    "Company name is required.");
            }

            return Result<bool>.Success(true);
        }

        private async Task<Result<bool>>
            ValidateTargetUserUpdatePermissionAsync(User user)
        {
            if (IsCurrentUserInRole(RoleName.SuperAdmin))
            {
                return Result<bool>.Success(true);
            }

            var roles = await userManager.GetRolesAsync(user);

            if (roles.Count == 1 && roles.Contains(RoleName.Citizen))
            {
                return Result<bool>.Success(true);
            }

            return Result<bool>.Failure(
                "Only a super administrator can update staff accounts.");
        }

        private bool IsCurrentUserInRole(string role)
        {
            var principal = httpContextAccessor.HttpContext?.User;

            return principal?.Identity?.IsAuthenticated == true &&
                   principal.IsInRole(role);
        }
    }
}
