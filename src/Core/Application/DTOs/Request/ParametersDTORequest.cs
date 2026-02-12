using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Request
{
    public class ParametersDTORequest
    {

        /// <summary>
        /// Gets or sets the TypeParameter.
        /// </summary>
        /// <value>The type of the parameter.</value>
        [Required(AllowEmptyStrings = false, ErrorMessage = "El tipo de parámetro es requerido")]
        [MaxLength(100, ErrorMessage = "El tipo de parámetro debe ser menor a 100 caracteres")]
        [DisplayName("Tipo de Parámetro")]
        public string TypeParameter { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the CreationDate.
        /// </summary>
        /// <value>The creation date.</value>
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayName("Fecha de Creación")]
        public DateTime CreationDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the ModifierDate.
        /// </summary>
        /// <value>The modification date.</value>
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayName("Fecha de Modificación")]
        public DateTime ModifierDate { get; set; }

        /// <summary>
        /// Gets or sets the CreationUser.
        /// </summary>
        /// <value>The user who created the parameter.</value>
        [Required(AllowEmptyStrings = false, ErrorMessage = "El usuario de creación es requerido")]
        [MaxLength(100, ErrorMessage = "El nombre del usuario de creación debe ser menor a 100 caracteres")]
        [DisplayName("Usuario de Creación")]
        public string CreationUser { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the ModifierUser.
        /// </summary>
        /// <value>The user who made the modification.</value>
        [Required(AllowEmptyStrings = false, ErrorMessage = "El usuario de modificación es requerido")]
        [MaxLength(100, ErrorMessage = "El nombre del usuario de modificación debe ser menor a 100 caracteres")]
        [DisplayName("Usuario de Modificación")]
        public string ModifierUser { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the Values.
        /// </summary>
        /// <value>The collection of values associated with the parameter.</value>
        [DisplayName("Valores")]
        public ICollection<ValueDTORequest>? Values { get; set; }
    }
}
