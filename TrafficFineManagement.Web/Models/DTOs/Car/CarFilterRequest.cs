using System.ComponentModel.DataAnnotations;
using TrafficFineManagement.Web.Models.Enums;

namespace TrafficFineManagement.Web.Models.DTOs.Car
{
    public class CarFilterRequest
    {
        [EnumDataType(typeof(CarType), ErrorMessage = "Geçerli bir araç tipi seçiniz.")]
        public CarType? CarType { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Sayfa numarası en az 1 olmalıdır.")]
        public int PageNumber { get; set; } = 1;

        [Range(1, 100, ErrorMessage = "Sayfa boyutu 1 ile 100 arasında olmalıdır.")]
        public int PageSize { get; set; } = 20;
    }
}
