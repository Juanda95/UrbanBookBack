using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Request
{
    /// <summary>
    /// DTO de solicitud para login público de clientes.
    /// El cliente se identifica con su correo o teléfono más su número de documento.
    /// </summary>
    public class PublicLoginDTORequest
    {
        /// <summary>
        /// Correo electrónico del cliente (opcional si se proporciona teléfono).
        /// </summary>
        [DisplayName("Correo")]
        [EmailAddress(ErrorMessage = "El correo debe ser una dirección válida")]
        public string? Correo { get; set; }

        /// <summary>
        /// Teléfono del cliente (opcional si se proporciona correo).
        /// </summary>
        [DisplayName("Teléfono")]
        public string? Telefono { get; set; }

        /// <summary>
        /// Número de documento del cliente (usado como identificación).
        /// </summary>
        [DisplayName("Número de Documento")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "El número de documento es requerido")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string NumeroDocumento { get; set; } = string.Empty;
    }
}
