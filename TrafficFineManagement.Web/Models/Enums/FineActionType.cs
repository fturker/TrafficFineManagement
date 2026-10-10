using System.ComponentModel.DataAnnotations;

namespace TrafficFineManagement.Web.Models.Enums
{
    public enum FineActionType
    {
        [Display(Name = "Onaylandı")]
        Approved = 0,

        [Display(Name = "Reddedildi")]
        Rejected = 1,
    }
}
