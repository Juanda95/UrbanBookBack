namespace Application.DTOs.Response
{
    /// <summary>
    /// DTO de respuesta para un servicio del catálogo.
    /// </summary>
    public class ServicioDTOResponse
    {
        /// <summary>
        /// Identificador único del servicio.
        /// </summary>
        public int ServicioId { get; set; }

        /// <summary>
        /// Nombre del servicio.
        /// </summary>
        public string Nombre { get; set; } = string.Empty;

        /// <summary>
        /// Descripción del servicio.
        /// </summary>
        public string Descripcion { get; set; } = string.Empty;

        /// <summary>
        /// Precio del servicio.
        /// </summary>
        public decimal Precio { get; set; }

        /// <summary>
        /// Duración estimada del servicio en minutos.
        /// </summary>
        public int DuracionMinutos { get; set; }

        /// <summary>
        /// URL de la imagen representativa del servicio.
        /// </summary>
        public string? ImagenUrl { get; set; }
    }
}
