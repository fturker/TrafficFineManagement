using System.ComponentModel.DataAnnotations;

namespace TrafficFineManagement.Web.Models.Enums
{
    public enum FineStatus
    {
        [Display(Name = "Yönetici onayı bekleniyor.")]
        New = 0,

        [Display(Name = "Finans onayı bekleniyor.")]
        ManagerApproved = 1,

        [Display(Name = "Tamamlandı.")]
        Completed = 2,

        [Display(Name = "Reddedildi.")]
        Rejected = 3,
    }
}
