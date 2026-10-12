using TrafficFineManagement.Web.Models.Enums;

namespace TrafficFineManagement.Web.Models.DTOs.Car
{
    public class CarListResponse
    {
        public int Id { get; set; }

        public string Plate { get; set; } = string.Empty;
        public CarType CarType { get; set; }

        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;

        public string? OwnerDisplayName { get; set; }
        public string? CompanyName { get; set; }
    }
}
