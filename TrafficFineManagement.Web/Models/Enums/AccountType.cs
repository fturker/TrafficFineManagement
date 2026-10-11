using System.ComponentModel.DataAnnotations;

namespace TrafficFineManagement.Web.Models.Enums
{
    public enum AccountType
    {
        [Display(Name = "Bireysel")]
        Individual = 1,

        [Display(Name = "Ticari işletme")]
        Business = 2
    }
}
