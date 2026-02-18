using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Request
{
    /// <summary>
    /// DTO para guardar todos los horarios de atención de un usuario en bulk.
    /// </summary>
    public class HorarioAtencionBulkDTORequest
    {
        [Required(ErrorMessage = "El ID del usuario es requerido")]
        public int UsuarioId { get; set; }

        [Required(ErrorMessage = "La lista de horarios es requerida")]
        public List<HorarioAtencionDTORequest> Horarios { get; set; } = [];
    }

    /// <summary>
    /// DTO para un día específico del horario de atención.
    /// </summary>
    public class HorarioAtencionDTORequest
    {
        [Required(ErrorMessage = "El ID del usuario es requerido")]
        public int UsuarioId { get; set; }

        [Required(ErrorMessage = "El día de la semana es requerido")]
        [Range(0, 6, ErrorMessage = "El día de la semana debe estar entre 0 (Domingo) y 6 (Sábado)")]
        public int DiaSemana { get; set; }

        public bool Activo { get; set; } = true;

        [Required(ErrorMessage = "La hora de inicio es requerida")]
        public TimeSpan HoraInicio { get; set; }

        [Required(ErrorMessage = "La hora de fin es requerida")]
        public TimeSpan HoraFin { get; set; }

        public List<ExclusionHorarioDTORequest> Exclusiones { get; set; } = [];
    }

    /// <summary>
    /// DTO para una exclusión dentro de un día laboral.
    /// </summary>
    public class ExclusionHorarioDTORequest
    {
        [Required(ErrorMessage = "La hora de inicio de la exclusión es requerida")]
        public TimeSpan HoraInicio { get; set; }

        [Required(ErrorMessage = "La hora de fin de la exclusión es requerida")]
        public TimeSpan HoraFin { get; set; }

        [MaxLength(200, ErrorMessage = "La descripción debe tener menos de 200 caracteres")]
        public string? Descripcion { get; set; }
    }
}
