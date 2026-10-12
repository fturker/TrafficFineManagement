using System.ComponentModel.DataAnnotations;

namespace TrafficFineManagement.Web.Rules
{
    public class RequestValidationRules
    {
        public static string? Validate(object request)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(request);

            var isValid = Validator.TryValidateObject(
                request,
                context,
                results,
                validateAllProperties: true);

            if (isValid)
            {
                return null;
            }

            return results.FirstOrDefault()?.ErrorMessage
                ?? "Invalid request.";
        }
    }
}
