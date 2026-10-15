using TrafficFineManagement.Web.Data.Repositories.Interfaces;
using TrafficFineManagement.Web.Models.Common;
using TrafficFineManagement.Web.Models.Entities;
using TrafficFineManagement.Web.Rules.Interfaces;
using TrafficFineManagement.Web.Services.Interfaces;

namespace TrafficFineManagement.Web.Services.Implementations
{
    public class PaymentService(IPaymentRepository paymentRepository, IFineRepository fineRepository, IPaymentBusinessRules paymentBusinessRules) : IPaymentService
    {
        public async Task<Result<bool>> PayAsync(int fineId, string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return Result<bool>.Failure("Kullanıcı bulunamadı.");
            }

            var fine = await fineRepository.GetByIdAsync(fineId);

            if(fine is null || fine.Car.OwnerId != userId)
            {
                return Result<bool>.Failure("Ceza bulunamadı ya da cezayı ödeme yetkiniz bulunmamaktadır.");
            }

            var ruleResult = paymentBusinessRules.ValidatePayment(fine);

            if (!ruleResult.IsSuccess)
                return ruleResult;

            var now = DateTime.UtcNow;

            var payment = new Payment
            {
                FineId = fine.Id,
                Fine = fine,

                UserId = fine.Car.OwnerId,
                Name = fine.Car.User.Name,
                Surname = fine.Car.User.Surname,

                Amount = fine.Amount,
                LastSixCardNumber = string.Empty,
                CreatedAt = now
            };

            fine.IsPaid = true;
            fine.PaidTime = now;
            fine.LastUpdatedAt = now;

            paymentRepository.Add(payment);

            await paymentRepository.SaveChangesAsync();

            return Result<bool>.Success(true);
        }
    }
}
