using System.ComponentModel.DataAnnotations;

namespace TrafficFineManagement.Web.Models.DTOs.User
{
    public class UpdateUserRequest
    {
        [Required]
        [StringLength(40)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(40)]
        public string Surname { get; set; } = string.Empty;

        [StringLength(200)]
        public string? CompanyName { get; set; }

        [StringLength(250)]
        public string? Address { get; set; }

        public DateTime? BirthDate { get; set; }
    }
}
