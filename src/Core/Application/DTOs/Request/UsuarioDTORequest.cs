using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Application.DTOs.Request
{
    public class UsuarioDTORequest
    {
        /// <summary>
        /// Correo electronico.
        /// </summary>
        [DisplayName("Correo")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "El Correo es requerido")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [MaxLength(100, ErrorMessage = "El Correo debe tener menos de 100 caracteres")]
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

        /// <summary>
        /// Nombre del usuario.
        /// </summary>
        [DisplayName("Nombre")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "El nombre es requerido")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [MaxLength(50, ErrorMessage = "El nombre debe tener menos de 50 caracteres")]
        public string Nombre { get; set; } = string.Empty;

        /// <summary>
        /// Apellido del usuario.
        /// </summary>
        [DisplayName("Apellido")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "El apellido es requerido")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [MaxLength(50, ErrorMessage = "El apellido debe tener menos de 50 caracteres")]
        public string Apellido { get; set; } = string.Empty;

        /// <summary>
        /// Dirección del usuario.
        /// </summary>
        [DisplayName("Dirección")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "La dirección es requerida")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [MaxLength(100, ErrorMessage = "La dirección debe tener menos de 100 caracteres")]
        public string Direccion { get; set; } = string.Empty;

        /// <summary>
        /// Teléfono del usuario.
        /// </summary>
        [DisplayName("Teléfono")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "El teléfono es requerido")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [MaxLength(15, ErrorMessage = "El teléfono debe tener menos de 15 caracteres")]
        public string Telefono { get; set; } = string.Empty;

        /// <summary>
        /// Lista de perfiles asociados al usuario.
        /// Los perfiles definen roles o grupos a los que el usuario pertenece.
        /// </summary>
        [DisplayName("Perfiles")]
        public IEnumerable<PerfilDTORequest> Perfiles { get; set; } = new HashSet<PerfilDTORequest>();
    }
}
