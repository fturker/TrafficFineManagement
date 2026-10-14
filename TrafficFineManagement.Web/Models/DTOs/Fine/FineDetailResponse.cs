using TrafficFineManagement.Web.Models.Enums;

namespace TrafficFineManagement.Web.Models.DTOs.Fine
{
    public class FineDetailResponse
    {
        public int Id { get; set; }

        public int CarId { get; set; }
        public string Plate { get; set; } = string.Empty;

        public decimal Amount { get; set; }

        public string Description { get; set; } = string.Empty;

        public DateTime FineDate { get; set; }

        public FineStatus Status { get; set; }

        public bool IsPaid { get; set; }
        public DateTime? PaidTime { get; set; }

        public string CreatedByUserId { get; set; } = string.Empty;
        public string CreatedByUserName { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }

        public List<FineHistoryItemResponse> Histories { get; set; } = new();
    }
}
