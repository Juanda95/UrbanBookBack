namespace Domain.Entities.DUsuario
{
    /// <summary>
    /// Representa el horario de atención de un profesional para un día específico de la semana.
    /// </summary>
    public class HorarioAtencion
    {
        public int HorarioAtencionId { get; set; }

        public int UsuarioId { get; set; }

        /// <summary>
        /// Día de la semana (0=Domingo, 1=Lunes, ..., 6=Sábado). Corresponde a DayOfWeek.
        /// </summary>
        public int DiaSemana { get; set; }

        /// <summary>
        /// Indica si el profesional atiende este día.
        /// </summary>
        public bool Activo { get; set; } = true;

        /// <summary>
        /// Hora de inicio de la jornada laboral.
        /// </summary>
        public TimeSpan HoraInicio { get; set; }

        /// <summary>
        /// Hora de fin de la jornada laboral.
        /// </summary>
        public TimeSpan HoraFin { get; set; }

        public virtual Usuario? Usuario { get; set; }

        public virtual List<ExclusionHorario> Exclusiones { get; set; } = [];
    }
}
