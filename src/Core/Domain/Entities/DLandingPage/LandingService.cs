using Domain.Entities.DNegocio;

namespace Domain.Entities.DLandingPage
{
    /// <summary>
    /// Servicio mostrado en la seccion de servicios del landing page.
    /// Cada servicio tiene un icono (react-icon o imagen custom), titulo y descripcion.
    /// </summary>
    public class LandingService : ITenantEntity
    {
        public int NegocioId { get; set; }
        public Negocio? Negocio { get; set; }

        public int LandingServiceId { get; set; }
        public int LandingConfigId { get; set; }
        public virtual LandingConfig? LandingConfig { get; set; }

        public int Orden { get; set; }
        public string IconCode { get; set; } = string.Empty;
        public string Titulo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
    }
}
