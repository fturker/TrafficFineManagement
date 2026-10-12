using TrafficFineManagement.Web.Models.Enums;

namespace TrafficFineManagement.Web.Models.DTOs.User
{
    public class UserResponse
    {
        public string Id { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;

        public AccountType AccountType { get; set; }

        public string? IdentityNumber { get; set; }
        public string? TaxNumber { get; set; }
        public string? CompanyName { get; set; }

        public string? Address { get; set; }
        public DateTime? BirthDate { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }
    }
}
