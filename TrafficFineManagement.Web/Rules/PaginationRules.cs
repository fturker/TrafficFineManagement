using TrafficFineManagement.Web.Models.Common;

namespace TrafficFineManagement.Web.Rules
{
    public class PaginationRules
    {
        public static Result<bool> Validate(
            int pageNumber,
            int pageSize)
        {
            if (pageNumber < 1)
            {
                return Result<bool>.Failure(
                    "Page number must be at least 1.");
            }

            if (pageSize < 1 || pageSize > 100)
            {
                return Result<bool>.Failure(
                    "Page size must be between 1 and 100.");
            }

            var skip = ((long)pageNumber - 1) * pageSize;

            if (skip > int.MaxValue)
            {
                return Result<bool>.Failure(
                    "The page number exceeds the supported limit.");
            }

            return Result<bool>.Success(true);
        }
    }
}
