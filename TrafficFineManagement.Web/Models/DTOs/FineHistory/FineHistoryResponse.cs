using TrafficFineManagement.Web.Models.Enums;

namespace TrafficFineManagement.Web.Models.DTOs.FineHistory
{
    public class FineHistoryResponse
    {
        public int Id { get; set; }

        public int FineId { get; set; }

        public string UserId { get; set; } = string.Empty;
        public string UserDisplayName { get; set; } = string.Empty;

        public FineActionType ActionType { get; set; }

        public string Description { get; set; } = string.Empty;

        public FineStatus? PreviousStatus { get; set; }
        public FineStatus NewStatus { get; set; }
        public FineStatus NextStatus { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
