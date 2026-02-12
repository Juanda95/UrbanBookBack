namespace Application.DTOs.Response
{
    /// <summary>
    /// DTO de respuesta con datos públicos del profesional (sin información sensible).
    /// </summary>
    public class ProfessionalPublicDTOResponse
    {
        /// <summary>
        /// Identificador único del profesional.
        /// </summary>
        public int UsuarioId { get; set; }

        /// <summary>
        /// Nombre del profesional.
        /// </summary>
        public string Nombre { get; set; } = string.Empty;

        /// <summary>
        /// Apellido del profesional.
        /// </summary>
        public string Apellido { get; set; } = string.Empty;

        /// <summary>
        /// URL del avatar del profesional (opcional).
        /// </summary>
        public string? AvatarUrl { get; set; }

        /// <summary>
        /// Especialidad del profesional (opcional).
        /// </summary>
        public string? Especialidad { get; set; }
    }
}
