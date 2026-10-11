using TrafficFineManagement.Web.Models.Enums;

namespace TrafficFineManagement.Web.Models.Entities
{
    public class Car : UpdatableEntity
    {
        public string Plate { get; set; } = string.Empty;
        public CarType CarType { get; set; }

        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;

        public string OwnerId { get; set; } = string.Empty;
        public User User { get; set; } = null!;

        public ICollection<Fine> Fines { get; set; } = new List<Fine>();
    }
}
