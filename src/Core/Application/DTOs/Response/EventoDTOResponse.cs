namespace Application.DTOs.Response
{
    public class EventoDTOResponse
    {
        /// <summary>
        /// Identificador único del Evento.
        /// </summary>
        public int EventoId { get; set; }

        /// <summary>
        /// Identificador del cliente asociado al evento.
        /// Se incluye para eficiencia y proyecciones.
        /// </summary>
        public int ClienteId { get; set; }

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
        public int StateProcessEventId { get; set; }

        /// <summary>
        /// Obtiene o establece la fecha y hora de inicio del evento.
        /// </summary>
        public DateTime FechaInicio { get; set; }

        /// <summary>
        /// Obtiene o establece la fecha y hora de finalización del evento.
        /// </summary>
        public DateTime FechaFin { get; set; }

        /// <summary>
        /// Obtiene o establece el motivo de cancelación del evento.
        /// </summary>
        /// <remarks>
        /// Este campo contiene la razón por la cual se canceló el evento.
        /// Es nulo si el evento no ha sido cancelado.
        /// </remarks>
        public string? Motivo { get; set; }

        /// <summary>
        /// Información básica del cliente asociado (proyección sin datos sensibles).
        /// </summary>
        public ClienteEventoDTOResponse? Cliente { get; set; }

    }

    public class EventoHistoryDTOResponse
    {
        /// <summary>
        /// Identificador único del Evento.
        /// </summary>
        public int EventoId { get; set; }

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
        /// Obtiene o establece la fecha y hora de inicio del evento.
        /// </summary>
        public DateTime FechaInicio { get; set; }

        /// <summary>
        /// Obtiene o establece la fecha y hora de finalización del evento.
        /// </summary>
        public DateTime FechaFin { get; set; }

        /// <summary>
        /// Obtiene o establece el motivo de cancelación del evento.
        /// </summary>
        /// <remarks>
        /// Este campo contiene la razón por la cual se canceló el evento.
        /// Es nulo si el evento no ha sido cancelado.
        /// </remarks>
        public string? Motivo { get; set; }

    }
}
