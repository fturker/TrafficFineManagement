using TrafficFineManagement.Web.Models.Common;
using TrafficFineManagement.Web.Models.Entities;
using TrafficFineManagement.Web.Models.Enums;
using TrafficFineManagement.Web.Rules.Interfaces;

namespace TrafficFineManagement.Web.Rules.Implementations
{
    public class PaymentBusinessRules : IPaymentBusinessRules
    {
        public Result<bool> ValidatePayment(Fine fine)
        {
            if (fine.Status != FineStatus.Completed)
            {
                return Result<bool>.Failure(
                    "Sadece onaylanan cezalar ödenebilir.");
            }

            if (fine.IsPaid)
            {
                return Result<bool>.Failure(
                    "Ceza halihazırda ödenmiştir.");
            }

            return Result<bool>.Success(true);
        }
    }
}
