using Domain.Entities.DNegocio;

namespace Domain.Entities.DLandingPage
{
    /// <summary>
    /// Item de la galeria del landing page. Puede ser imagen o video.
    /// </summary>
    public class LandingGalleryItem : ITenantEntity
    {
        public int NegocioId { get; set; }
        public Negocio? Negocio { get; set; }

        public int LandingGalleryItemId { get; set; }
        public int LandingConfigId { get; set; }
        public virtual LandingConfig? LandingConfig { get; set; }

        public int Orden { get; set; }
        public string MediaType { get; set; } = "image";
        public string FileName { get; set; } = string.Empty;
        public string? AltText { get; set; }
    }
}
