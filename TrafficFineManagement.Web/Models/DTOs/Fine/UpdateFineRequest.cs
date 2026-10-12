namespace TrafficFineManagement.Web.Models.DTOs.Fine
{
    public class UpdateFineRequest
    {
        public decimal Amount { get; set; }

        public string Description { get; set; } = string.Empty;

        public DateTime FineDate { get; set; }
    }
}
