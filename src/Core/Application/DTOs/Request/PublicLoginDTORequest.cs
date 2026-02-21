using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Request
{
    /// <summary>
    /// DTO de solicitud para login público de clientes.
    /// El cliente se identifica con su número de teléfono.
    /// </summary>
    public class PublicLoginDTORequest
    {
        /// <summary>
        /// Teléfono del cliente (requerido).
        /// </summary>
        [Required(ErrorMessage = "El teléfono es requerido")]
        [DisplayName("Teléfono")]
        public string Telefono { get; set; } = string.Empty;
    }
}
