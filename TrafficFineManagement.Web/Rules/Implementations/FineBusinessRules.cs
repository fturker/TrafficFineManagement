using Microsoft.AspNetCore.Identity;
using TrafficFineManagement.Web.Consts;
using TrafficFineManagement.Web.Models.Common;
using TrafficFineManagement.Web.Models.Entities;
using TrafficFineManagement.Web.Models.Enums;
using TrafficFineManagement.Web.Rules.Interfaces;

namespace TrafficFineManagement.Web.Rules.Implementations
{
    public class FineBusinessRules(UserManager<User> userManager) : IFineBusinessRules
    {
        public async Task<Result<bool>> ValidateDecisionAsync(
            Fine fine,
            User user,
            FineStatus? expectedStatus)
        {
            if (!expectedStatus.HasValue || fine.Status != expectedStatus.Value)
            {
                return Result<bool>.Failure(
                    "Ceza durumu güncellendi. Sayfayı yenileyiniz");
            }

            if (fine.IsPaid)
            {
                return Result<bool>.Failure(
                    "Ödenmiş ceza onaylanamaz ya da reddedilemez.");
            }

            var roles = await userManager.GetRolesAsync(user);

            if (roles.Contains(RoleName.SuperAdmin))
                return Result<bool>.Success(true);

            var requiredRole = fine.Status == FineStatus.New
                ? RoleName.Admin
                : RoleName.Finance;

            if (!roles.Contains(requiredRole))
            {
                return Result<bool>.Failure(
                    "Bu aşamayı onaylacak yetkiniz bulunmamaktadır.");
            }

            return Result<bool>.Success(true);
        }

        public Result<bool> ValidateRejectionReason(string? reason)
        {
            if (string.IsNullOrWhiteSpace(reason))
            {
                return Result<bool>.Failure(
                    "Açıklama gereklidir");
            }

            return Result<bool>.Success(true);
        }

        public async Task<Result<bool>> ValidateUpdateAsync(
            Fine fine,
            User user)
        {
            if (fine.Status != FineStatus.New || fine.IsPaid)
            {
                return Result<bool>.Failure(
                    "Sadece ödenmemiş ve admin onayı bekleyen cezalar güncellenebilir.");
            }

            var roles = await userManager.GetRolesAsync(user);

            var canUpdate = roles.Contains(RoleName.SuperAdmin)
                || (roles.Contains(RoleName.Officer)
                    && fine.CreatedByUserId == user.Id);

            if (!canUpdate)
            {
                return Result<bool>.Failure(
                    "Bu cezayı güncelleme yetkiniz bulunmamaktadır.");
            }

            return Result<bool>.Success(true);
        }
    }
}
