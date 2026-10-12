using System.ComponentModel.DataAnnotations;

namespace TrafficFineManagement.Web.Models.DTOs.Auth
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "TC no veya Vergi no girilmesi zorunludur.")]
        [StringLength(11, ErrorMessage ="Kullanıcı adı en fazla 11 haneli olabilir")]
        public string Identifier { get; set; } = string.Empty;

        [Required(ErrorMessage = "Şifre girilmesi zorunludur.")]
        [StringLength(8, ErrorMessage ="Parola en fazla 8 haneli olabilir")]
        public string Password { get; set; } = string.Empty;
    }
}
