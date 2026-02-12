using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Request
{
    public class EventoDTOUpdateRequest
    {

        /// <summary>
        /// Identificador único del Evento.
        /// </summary>
        [Required(ErrorMessage = "El ID del evento es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID del evento debe ser un número positivo mayor que cero")]
        public int EventoId { get; set; }

        /// <summary>
        /// Obtiene o establece el título del evento.
        /// </summary>
        [DisplayName("Título del Evento")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "El título es requerido")]
        [MaxLength(100, ErrorMessage = "El título debe ser menor a 100 caracteres")]
        public string Titulo { get; set; } = string.Empty;

        /// <summary>
        /// Obtiene o establece la descripción del evento.
        /// </summary>
        [DisplayName("Descripción del Evento")]
        [MaxLength(1000, ErrorMessage = "La descripción debe ser menor a 1000 caracteres")]
        public string Descripcion { get; set; } = string.Empty;

        /// <summary>
        /// Obtiene o establece el estado del evento.
        /// </summary>
        /// <remarks>
        /// El estado es verdadero si el evento está activo, y falso si está inactivo.
        /// </remarks>
        [DisplayName("Estado del Evento")]
        [Required(ErrorMessage = "El estado es requerido")]
        public bool Estado { get; set; }

        /// <summary>
        /// state of the process of the event.
        /// </summary>
        [DisplayName("Estado del Proceso")]
        [Required(ErrorMessage = "El estado del proceso es requerido")]
        public int StateProcessEventId { get; set; }

        /// <summary>
        /// Obtiene o establece la fecha y hora de inicio del evento.
        /// </summary>
        [DisplayName("Fecha de Inicio")]
        [Required(ErrorMessage = "La fecha de inicio es requerida")]
        public DateTime FechaInicio { get; set; }

        /// <summary>
        /// Obtiene o establece la fecha y hora de finalización del evento.
        /// </summary>
        [DisplayName("Fecha de Fin")]
        [Required(ErrorMessage = "La fecha de fin es requerida")]
        public DateTime FechaFin { get; set; }

        /// <summary>
        /// Obtiene o establece el motivo de cancelación del evento.
        /// </summary>
        /// <remarks>
        /// Este campo es opcional y solo debe proporcionarse cuando se cancela un evento.
        /// </remarks>
        [DisplayName("Motivo de Cancelación")]
        [MaxLength(500, ErrorMessage = "El motivo debe ser menor a 500 caracteres")]
        public string? Motivo { get; set; }

    }
}
