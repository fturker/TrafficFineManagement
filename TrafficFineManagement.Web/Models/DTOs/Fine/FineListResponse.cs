using TrafficFineManagement.Web.Models.Enums;

namespace TrafficFineManagement.Web.Models.DTOs.Fine
{
    public class FineListResponse
    {
        public int Id { get; set; }

        public int CarId { get; set; }
        public string Plate { get; set; } = string.Empty;

        public decimal Amount { get; set; }

        public string Description { get; set; } = string.Empty;

        public DateTime FineDate { get; set; }

        public FineStatus Status { get; set; }

        public bool IsPaid { get; set; }
    }
}
