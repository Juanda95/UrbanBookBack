using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Request
{
    public class PerfilDTORequest
    {
        /// <summary>
        /// Identificador único para el perfil.
        /// </summary>
        [Required(ErrorMessage = "El ID del perfil es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID del perfil debe ser un número positivo mayor que cero")]
        public int PerfilId { get; set; }
   
    }
}
