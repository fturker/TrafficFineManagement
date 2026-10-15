using TrafficFineManagement.Web.Models.Common;
using TrafficFineManagement.Web.Models.Entities;

namespace TrafficFineManagement.Web.Rules.Interfaces
{
    public interface IPaymentBusinessRules
    {
        Result<bool> ValidatePayment(Fine fine);
    }
}
