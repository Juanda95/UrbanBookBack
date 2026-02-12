using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Request
{
    public class LoginDTORequest
    {
        /// <summary>
        /// Correo electronico.
        /// </summary>
        [DisplayName("Correo")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "El Correo es requerido")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [MaxLength(150, ErrorMessage = "El Correo debe tener menos de 100 caracteres")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Contraseña del usuario.
        /// Nota: Debe ser manejada con cuidado y, si es posible, almacenada en formato cifrado o hash.
        /// </summary>
        [DisplayName("Contraseña")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "La contraseña es requerida")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [MaxLength(50, ErrorMessage = "La contraseña debe ser menor a 50 caracteres")]
        public string Password { get; set; } = string.Empty;

    }
}
