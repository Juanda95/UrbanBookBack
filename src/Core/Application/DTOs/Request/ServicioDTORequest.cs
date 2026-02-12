using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Request
{
    /// <summary>
    /// DTO de solicitud para crear o actualizar un servicio del catálogo.
    /// </summary>
    public class ServicioDTORequest
    {
        /// <summary>
        /// Nombre del servicio.
        /// </summary>
        [DisplayName("Nombre del Servicio")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "El nombre del servicio es requerido")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [MaxLength(100, ErrorMessage = "El nombre debe tener menos de 100 caracteres")]
        public string Nombre { get; set; } = string.Empty;

        /// <summary>
        /// Descripción del servicio.
        /// </summary>
        [DisplayName("Descripción")]
        [MaxLength(500, ErrorMessage = "La descripción debe tener menos de 500 caracteres")]
        public string Descripcion { get; set; } = string.Empty;

        /// <summary>
        /// Precio del servicio.
        /// </summary>
        [DisplayName("Precio")]
        [Required(ErrorMessage = "El precio es requerido")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0")]
        public decimal Precio { get; set; }

        /// <summary>
        /// Duración estimada del servicio en minutos.
        /// </summary>
        [DisplayName("Duración (minutos)")]
        [Required(ErrorMessage = "La duración es requerida")]
        [Range(15, 480, ErrorMessage = "La duración debe estar entre 15 y 480 minutos")]
        public int DuracionMinutos { get; set; }

        /// <summary>
        /// URL de la imagen representativa del servicio.
        /// </summary>
        [MaxLength(500, ErrorMessage = "La URL de la imagen debe tener menos de 500 caracteres")]
        public string? ImagenUrl { get; set; }
    }
}
