namespace TrafficFineManagement.Web.Models.DTOs.Payment
{
    public class PaymentResponse
    {
        public int FineId { get; set; }

        public string Plate { get; set; } = string.Empty;

        public decimal Amount { get; set; }

        public string Description { get; set; } = string.Empty;
    }
}