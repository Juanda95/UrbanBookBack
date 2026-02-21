using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Request
{
    public class VerifyOtpDTORequest
    {
        [Required(ErrorMessage = "El teléfono es requerido")]
        [DisplayName("Teléfono")]
        public string Telefono { get; set; } = string.Empty;

        [Required(ErrorMessage = "El código es requerido")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "El código debe ser de 6 dígitos")]
        [DisplayName("Código")]
        public string Code { get; set; } = string.Empty;
    }
}
