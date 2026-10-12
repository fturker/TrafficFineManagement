using TrafficFineManagement.Web.Models.Enums;

namespace TrafficFineManagement.Web.Models.DTOs.Car
{
    public class CarDetailResponse
    {
        public int Id { get; set; }

        public string Plate { get; set; } = string.Empty;
        public CarType CarType { get; set; }

        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;

        public string OwnerId { get; set; } = string.Empty;
        public string? OwnerName { get; set; }
        public string? OwnerSurname { get; set; }
        public string? IdentityNumber { get; set; }

        public string? CompanyName { get; set; }
        public string? TaxNumber { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }
    }
}
