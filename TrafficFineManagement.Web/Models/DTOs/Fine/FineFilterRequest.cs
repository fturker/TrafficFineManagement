using TrafficFineManagement.Web.Models.Enums;

namespace TrafficFineManagement.Web.Models.DTOs.Fine
{
    public class FineFilterRequest
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;

        public string? SearchTerm { get; set; }

        public int? CarId { get; set; }

        public FineStatus? Status { get; set; }

        public bool? IsPaid { get; set; }
    }
}
