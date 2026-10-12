namespace TrafficFineManagement.Web.Models.DTOs.Fine
{
    public class CreateFineRequest
    {
        public int CarId { get; set; }

        public decimal Amount { get; set; }

        public string Description { get; set; } = string.Empty;

        public DateTime FineDate { get; set; }
    }
}
