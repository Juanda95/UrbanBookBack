using Domain.Entities.Dcliente;
using Domain.Entities.DNegocio;
using Domain.Entities.DServicio;
using Domain.Entities.DUsuario;

namespace Domain.Entities.DCalendario
{
    /// <summary>
    /// Representa un evento en un calendario.
    /// </summary>
    public class Evento : ITenantEntity
    {
        /// <summary>
        /// Identificador del negocio (tenant) al que pertenece este evento.
        /// </summary>
        public int NegocioId { get; set; }
        public Negocio? Negocio { get; set; }

        /// <summary>
        /// Identificador único del Evento.
        /// </summary>
        public int EventoId { get; set; }

        /// <summary>
        /// Identificador del cliente al que pertenece este contacto.
        /// </summary>
        public int ClienteId { get; set; }

        /// <summary>
        /// Identificador del Usuario al que pertenece este evento.
        /// </summary>
        public int UsuarioId { get; set; }


        /// <summary>
        /// Obtiene o establece el título del evento.
        /// </summary>
        public string Titulo { get; set; } = string.Empty;

        /// <summary>
        /// Obtiene o establece la descripción del evento.
        /// </summary>
        public string Descripcion { get; set; } = string.Empty;

        /// <summary>
        /// Obtiene o establece el estado del evento.
        /// </summary>
        /// <remarks>
        /// El estado es verdadero si el evento está activo, y falso si está inactivo.
        /// </remarks>
        public bool Estado { get; set; }

        /// <summary>
        /// State of the process of the event.
        /// </summary>
        public int? StateProcessEventId { get; set; }

        /// <summary>
        /// Obtiene o establece la fecha y hora de inicio del evento.
        /// </summary>
        public DateTime FechaInicio { get; set; }

        /// <summary>
        /// Obtiene o establece la fecha y hora de finalización del evento.
        /// </summary>
        public DateTime FechaFin { get; set; }

        /// <summary>
        /// Obtiene o establece la fecha y hora en que se registró el evento.
        /// </summary>
        public DateTime FechaRegistro { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the event has been scheduled.
        /// </summary>
        /// <value>
        /// True if the event has been scheduled; otherwise, false.
        /// </value>
        public bool IsScheduled { get; set; }

        /// <summary>
        /// Obtiene o establece el motivo de cancelación del evento.
        /// </summary>
        /// <remarks>
        /// Este campo almacena la razón por la cual se canceló el evento.
        /// Es nulo si el evento no ha sido cancelado.
        /// </remarks>
        public string? Motivo { get; set; }

        /// <summary>
        /// FK al servicio asociado a este evento (nullable para eventos legacy).
        /// </summary>
        public int? ServicioId { get; set; }

        /// <summary>
        /// Precio del servicio al momento de la reserva (snapshot).
        /// </summary>
        public decimal? Precio { get; set; }

        /// <summary>
        /// cliente al que pertenece este contacto.
        /// </summary>
        public virtual Cliente? Cliente { get; set; }

        /// <summary>
        /// Usuario asociado al evento
        /// </summary>
        public virtual Usuario? Usuario { get; set; }

        /// <summary>
        /// State of the process of the event.
        /// </summary>
        public virtual StateProcessEvents? StateProcessEvent { get; set; }

        /// <summary>
        /// Servicio asociado al evento.
        /// </summary>
        public virtual Servicio? Servicio { get; set; }

    }
}
