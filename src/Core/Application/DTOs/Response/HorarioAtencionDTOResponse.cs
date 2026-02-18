namespace Application.DTOs.Response
{
    /// <summary>
    /// DTO de respuesta para un día del horario de atención.
    /// </summary>
    public class HorarioAtencionDTOResponse
    {
        public int HorarioAtencionId { get; set; }
        public int UsuarioId { get; set; }
        public int DiaSemana { get; set; }
        public bool Activo { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFin { get; set; }
        public List<ExclusionHorarioDTOResponse> Exclusiones { get; set; } = [];
    }

    /// <summary>
    /// DTO de respuesta para una exclusión de horario.
    /// </summary>
    public class ExclusionHorarioDTOResponse
    {
        public int ExclusionHorarioId { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFin { get; set; }
        public string? Descripcion { get; set; }
    }
}
