using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Request
{
    public class SendOtpDTORequest
    {
        [Required(ErrorMessage = "El teléfono es requerido")]
        [DisplayName("Teléfono")]
        public string Telefono { get; set; } = string.Empty;

        public int? ClienteId { get; set; }
    }
}
