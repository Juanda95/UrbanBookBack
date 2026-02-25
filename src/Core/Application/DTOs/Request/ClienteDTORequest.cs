using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Request
{
    public class ClienteDTORequest
    {
        /// <summary>
        /// Nombre del cliente.
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "El nombre del cliente es requerido")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [MaxLength(100, ErrorMessage = "El nombre del cliente debe ser menor a 100 caracteres")]
        [DisplayName("Nombres")]
        public string Nombres { get; set; } = string.Empty;

        /// <summary>
        /// Primer apellido del cliente.
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "El primer apellido del cliente es requerido")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [MaxLength(100, ErrorMessage = "El primer apellido del cliente debe ser menor a 100 caracteres")]
        [DisplayName("Primer apellido")]
        public string PrimerApellido { get; set; } = string.Empty;

        /// <summary>
        /// Segundo apellido del cliente.
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "El segundo apellido del cliente es requerido")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [MaxLength(100, ErrorMessage = "El segundo apellido del cliente debe ser menor a 100 caracteres")]
        [DisplayName("Segundo apellido")]
        public string SegundoApellido { get; set; } = string.Empty;

        /// <summary>
        /// Número de documento del cliente (opcional).
        /// </summary>
        [MaxLength(100, ErrorMessage = "El número de documento del cliente debe ser menor a 100 caracteres")]
        [DisplayName("Número de documento")]
        public string? NumeroDocumento { get; set; }

        /// <summary>
        /// Teléfono del cliente.
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "El teléfono del cliente es requerido")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [MaxLength(100, ErrorMessage = "El teléfono del cliente debe ser menor a 100 caracteres")]
        [DisplayName("Teléfono")]
        public string Telefono { get; set; } = string.Empty;

        /// <summary>
        /// Correo del cliente.
        /// </summary>
        [MaxLength(100, ErrorMessage = "El correo del cliente debe ser menor a 100 caracteres")]
        [DisplayName("Correo")]
        public string? Correo { get; set; }
    }
}
