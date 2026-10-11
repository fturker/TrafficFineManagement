using Microsoft.AspNetCore.Identity;
using TrafficFineManagement.Web.Models.Enums;

namespace TrafficFineManagement.Web.Models.Entities
{
    public class User : IdentityUser
    {
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;

        public string? IdentityNumber { get; set; }

        public string? Address { get; set; }
        public DateTime? BirthDate { get; set; }

        public AccountType AccountType { get; set; } = AccountType.Individual;

        public string? CompanyName { get; set; }
        public string? TaxNumber { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime LastUpdatedAt { get; set; } = DateTime.Now;

        public ICollection<Car> Cars { get; set; } = new List<Car>();
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}
