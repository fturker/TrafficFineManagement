using Microsoft.AspNetCore.Identity;

namespace TrafficFineManagement.Web.Models.Entities
{
    public class User : IdentityUser
    {
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;

        public string? IdentityNumber { get; set; }

        public string? Address { get; set; }
        public DateTime? BirthDate { get; set; }

        public DateTime CreatedTime { get; set; } = DateTime.Now;
        public DateTime LastUpdatedTaime { get; set; } = DateTime.Now;

        public ICollection<Car> Cars { get; set; } = new List<Car>();
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}
