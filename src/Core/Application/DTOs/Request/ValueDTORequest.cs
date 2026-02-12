using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Request
{
    /// <summary>
    /// Represents a value associated with a parameter.
    /// </summary>
    public class ValueDTORequest 
    {

        /// <summary>
        /// Identificador único del parámetro.
        /// </summary>
        [DisplayName("ID valor")]
        public int IdValue { get; set; }

        /// <summary>
        /// Gets or sets the code of the value.
        /// </summary>
        /// <value>The code value.</value>
        [Required(ErrorMessage = "El código es requerido")]
        [DisplayName("Código")]
        [StringLength(100, ErrorMessage = "El código no debe exceder los 100 caracteres.")]
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the actual name.
        /// </summary>
        /// <value>The name.</value>
        [Required(ErrorMessage = "El name es requerido")]
        [DisplayName("Valor")]
        [StringLength(100, ErrorMessage = "El name no debe exceder los 100 caracteres.")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the description of the value.
        /// </summary>
        /// <value>The description of the value.</value>
        [Required(ErrorMessage = "la descripción  es requerida")]
        [DisplayName("Descripción")]
        [StringLength(200, ErrorMessage = "La descripcion no debe exceder los 200 caracteres.")]
        public string Description { get; set; } = string.Empty;
    }
}
