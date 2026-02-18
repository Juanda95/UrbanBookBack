namespace Domain.Entities.DUsuario
{
    /// <summary>
    /// Representa un rango de tiempo excluido dentro de un día laboral (ej: almuerzo).
    /// </summary>
    public class ExclusionHorario
    {
        public int ExclusionHorarioId { get; set; }

        public int HorarioAtencionId { get; set; }

        /// <summary>
        /// Hora de inicio de la exclusión.
        /// </summary>
        public TimeSpan HoraInicio { get; set; }

        /// <summary>
        /// Hora de fin de la exclusión.
        /// </summary>
        public TimeSpan HoraFin { get; set; }

        /// <summary>
        /// Descripción opcional de la exclusión (ej: "Almuerzo").
        /// </summary>
        public string? Descripcion { get; set; }

        public virtual HorarioAtencion? HorarioAtencion { get; set; }
    }
}
