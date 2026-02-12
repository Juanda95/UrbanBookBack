using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Request
{
    public class ParameterDTOUpdateRequest
    {
        /// <summary>
        /// Identificador único del parámetro.
        /// </summary>
        [Required(ErrorMessage = "El ID del parámetro es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID del parámetro debe ser un número positivo mayor que cero")]
        [DisplayName("ID Parámetro")]
        public int IdParameter { get; set; }

        /// <summary>
        /// Tipo del parámetro.
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "El tipo del parámetro es requerido")]
        [MaxLength(100, ErrorMessage = "El tipo del parámetro debe ser menor a 100 caracteres")]
        [DisplayName("Tipo de Parámetro")]
        public string TypeParameter { get; set; } = string.Empty;

        /// <summary>
        /// Fecha de creación del parámetro.
        /// </summary>
        /// No se incluye validación ya que generalmente la fecha de creación no se actualiza.
        [DisplayName("Fecha de Creación")]
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// Fecha de modificación del parámetro.
        /// </summary>
        /// Se podría omitir en la solicitud de actualización y establecer automáticamente en el backend.
        [DisplayName("Fecha de Modificación")]
        public DateTime ModifierDate { get; set; }

        /// <summary>
        /// Usuario que creó el parámetro.
        /// </summary>
        /// No se incluye validación ya que generalmente el usuario creador no se actualiza.
        [DisplayName("Usuario de Creación")]
        public string CreationUser { get; set; } = string.Empty;

        /// <summary>
        /// Usuario que modificó el parámetro.
        /// </summary>
        /// Se podría omitir en la solicitud de actualización y establecer automáticamente en el backend.
        [DisplayName("Usuario de Modificación")]
        public string ModifierUser { get; set; } = string.Empty;

        /// <summary>
        /// Valores asociados al parámetro.
        /// </summary>
        /// Se asume que la actualización puede incluir modificaciones a los valores asociados.
        [DisplayName("Valores")]
        public ICollection<ValueDTORequest>? Values { get; set; }
    }
}
