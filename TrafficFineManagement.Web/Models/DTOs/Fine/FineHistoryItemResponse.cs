using TrafficFineManagement.Web.Models.Enums;

namespace TrafficFineManagement.Web.Models.DTOs.Fine
{
    public class FineHistoryItemResponse
    {
        public DateTime CreatedAt { get; set; }

        public string UserDisplayName { get; set; } = string.Empty;

        public FineActionType ActionType { get; set; }

        public string Description { get; set; } = string.Empty;

        public FineStatus? PreviousStatus { get; set; }

        public FineStatus NewStatus { get; set; }
    }
}
