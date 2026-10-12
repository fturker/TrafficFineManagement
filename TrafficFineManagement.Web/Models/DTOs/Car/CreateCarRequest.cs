using System.ComponentModel.DataAnnotations;
using TrafficFineManagement.Web.Models.Enums;

namespace TrafficFineManagement.Web.Models.DTOs.Car
{
    public class CreateCarRequest
    {
        [Required(ErrorMessage = "Plaka zorunludur.")]
        [StringLength(9, ErrorMessage = "Plaka en fazla 9 karakter olabilir.")]
        public string Plate { get; set; } = string.Empty;

        [EnumDataType(typeof(CarType), ErrorMessage = "Geçerli bir araç tipi seçiniz.")]
        public CarType CarType { get; set; }

        [Required(ErrorMessage = "Marka zorunludur.")]
        [StringLength(20, ErrorMessage = "Marka en fazla 20 karakter olabilir.")]
        public string Brand { get; set; } = string.Empty;

        [Required(ErrorMessage = "Model zorunludur.")]
        [StringLength(20, ErrorMessage = "Model en fazla 20 karakter olabilir.")]
        public string Model { get; set; } = string.Empty;

        [Required(ErrorMessage = "Araç sahibi seçilmelidir.")]
        public string OwnerId { get; set; } = string.Empty;
    }
}
