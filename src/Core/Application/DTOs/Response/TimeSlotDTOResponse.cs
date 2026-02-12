namespace Application.DTOs.Response
{
    /// <summary>
    /// DTO de respuesta para un slot de tiempo disponible.
    /// </summary>
    public class TimeSlotDTOResponse
    {
        /// <summary>
        /// Fecha y hora de inicio del slot.
        /// </summary>
        public DateTime FechaInicio { get; set; }

        /// <summary>
        /// Fecha y hora de fin del slot.
        /// </summary>
        public DateTime FechaFin { get; set; }

        /// <summary>
        /// Indica si el slot está disponible para reserva.
        /// </summary>
        public bool Disponible { get; set; }
    }
}
