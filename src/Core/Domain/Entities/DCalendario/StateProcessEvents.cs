namespace Domain.Entities.DCalendario
{
    /// <summary>
    /// Represents the state process events in the calendar domain.
    /// </summary>
    public class StateProcessEvents
    {
        /// <summary>
        /// Gets or sets the unique identifier for the state process event.
        /// </summary>
        public int StateProcessEventsId { get; set; }

        /// <summary>
        /// Gets or sets the state of the process event.
        /// </summary>
        public string State { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the list of events associated with the state process.
        /// </summary>
        public virtual List<Evento> Eventos { get; set; } = [];
    }
}
