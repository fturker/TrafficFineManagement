using TrafficFineManagement.Web.Models.Enums;

namespace TrafficFineManagement.Web.Models.Entities
{
    public class Fine : UpdatableEntity
    {
        public int CarId { get; set; }
        public Car Car { get; set; } = null!;
        public decimal Amount { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime FineDate { get; set; } = DateTime.Now;
        public FineStatus Status { get; set; }
        public bool IsPaid { get; set; } = false;
        public DateTime? PaidTime { get; set; }
        public ICollection<FineHistory> Histories { get; set; } = new List<FineHistory>();
    }
}
