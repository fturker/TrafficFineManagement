using System.ComponentModel.DataAnnotations;
using TrafficFineManagement.Web.Models.Enums;

namespace TrafficFineManagement.Web.Models.DTOs.User
{
    public class UserFilterRequest
    {
        [Range(1, int.MaxValue)]
        public int PageNumber { get; set; } = 1;

        [Range(1, 100)]
        public int PageSize { get; set; } = 20;

        public string? SearchTerm { get; set; }

        [EnumDataType(typeof(AccountType))]
        public AccountType? AccountType { get; set; }
    }
}
