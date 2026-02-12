namespace Domain.Entities.DServicio
{
    /// <summary>
    /// Representa un servicio ofrecido por el negocio (ej: Corte de Pelo, Barba).
    /// Contiene información del nombre, descripción, precio y duración estimada del servicio.
    /// </summary>
    public class Servicio
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
        /// Descripción detallada del servicio.
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
        /// URL de la imagen representativa del servicio (opcional).
        /// </summary>
        public string? ImagenUrl { get; set; }

        /// <summary>
        /// Indica si el servicio está activo y disponible para reservas.
        /// </summary>
        public bool Activo { get; set; } = true;
    }
}
