using System.ComponentModel.DataAnnotations;
using TrafficFineManagement.Web.Models.Enums;

namespace TrafficFineManagement.Web.Models.DTOs.Fine
{
    public class FineDecisionRequest
    {
        [Required(ErrorMessage = "Ceza durumu zorunludur.")]
        [EnumDataType(typeof(FineStatus),
            ErrorMessage = "Geçersiz ceza durumu.")]
        public FineStatus? ExpectedStatus { get; set; }

        [StringLength(500,
            ErrorMessage = "Ret gerekçesi en fazla 500 karakter olabilir.")]
        public string? Reason { get; set; }
    }
}
