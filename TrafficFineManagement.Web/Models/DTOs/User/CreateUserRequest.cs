using System.ComponentModel.DataAnnotations;
using TrafficFineManagement.Web.Consts;
using TrafficFineManagement.Web.Models.Enums;

namespace TrafficFineManagement.Web.Models.DTOs.User
{
    public class CreateUserRequest
    {
        [StringLength(40)]
        public string Name { get; set; } = string.Empty;

        [StringLength(40)]
        public string Surname { get; set; } = string.Empty;

        [EnumDataType(typeof(AccountType))]
        public AccountType AccountType { get; set; } = AccountType.Individual;

        [RegularExpression(
            @"^[1-9][0-9]{10}$",
            ErrorMessage = "TC no 11 haneli olmalıdır ve 0 ile başlayamaz.")]
        public string? IdentityNumber { get; set; }

        [RegularExpression(
            @"^[0-9]{10}$",
            ErrorMessage = "Vergi numarası 10 haneli olmalıdır.")]
        public string? TaxNumber { get; set; }

        [StringLength(200)]
        public string? CompanyName { get; set; }

        [StringLength(250)]
        public string? Address { get; set; }

        public DateTime? BirthDate { get; set; }

        [StringLength(256)]
        public string? UserName { get; set; }

        [Required]
        public string Password { get; set; } = string.Empty;

        [Required]
        public string Role { get; set; } = RoleName.Citizen;
    }
}
