using TrafficFineManagement.Web.Models.Enums;

namespace TrafficFineManagement.Web.Models.Entities
{
    public class FineHistory : BaseEntity
    {
        public int FineId { get; set; }
        public Fine Fine { get; set; } = null!;
        public int UserId { get; set; }
        public FineActionType ActionType { get; set; }
        public string Description { get; set; } = string.Empty;
        public FineStatus PreviousStatus { get; set; }
        public FineStatus NewStatus { get; set; }
        public FineStatus NextStatus { get; set; }
    }
}
