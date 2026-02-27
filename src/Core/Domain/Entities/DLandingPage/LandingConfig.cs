using Domain.Entities.DNegocio;

namespace Domain.Entities.DLandingPage
{
    /// <summary>
    /// Configuracion principal del landing page de un negocio.
    /// Almacena hero (titulo, subtitulo, imagen), WhatsApp y relaciones con servicios y galeria.
    /// </summary>
    public class LandingConfig : ITenantEntity
    {
        public int NegocioId { get; set; }
        public Negocio? Negocio { get; set; }

        public int LandingConfigId { get; set; }

        public string? HeroTitle { get; set; }
        public string? HeroSubtitle { get; set; }
        public string? HeroImageFileName { get; set; }

        public string? WhatsAppNumber { get; set; }
        public string? WhatsAppMessage { get; set; }

        public bool Activo { get; set; } = true;
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public DateTime FechaModificacion { get; set; } = DateTime.UtcNow;

        // Navegacion
        public virtual List<LandingService> LandingServices { get; set; } = [];
        public virtual List<LandingGalleryItem> LandingGalleryItems { get; set; } = [];
    }
}
