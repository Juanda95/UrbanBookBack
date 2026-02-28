using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Request
{
    public class EventoDTORequest
    {
        /// <summary>
        /// Identificador único del Evento.
        /// </summary>
        [Required(ErrorMessage = "El ID del cliente es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID del cliente debe ser un número positivo mayor que cero")]
        public int ClienteId { get; set; }

        /// <summary>
        /// Identificador del Usuario al que pertenece este evento.
        /// </summary>
        [Required(ErrorMessage = "El ID del usuario es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID del usuario debe ser un número positivo mayor que cero")]
        public int UsuarioId { get; set; }

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
        public int? StateProcessEventId { get; set; }
         
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

        /// <summary>
        /// Identificador del servicio asociado (opcional).
        /// </summary>
        [DisplayName("Servicio")]
        public int? ServicioId { get; set; }

        /// <summary>
        /// Precio del servicio al momento de la reserva (opcional).
        /// </summary>
        [DisplayName("Precio")]
        public decimal? Precio { get; set; }

    }
}
