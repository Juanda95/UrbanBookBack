namespace Application.DTOs.Response
{
    public class LandingConfigDTOResponse
    {
        public int LandingConfigId { get; set; }
        public string HeroTitle { get; set; } = string.Empty;
        public string HeroSubtitle { get; set; } = string.Empty;
        public string? HeroImageFileName { get; set; }
        public string? WhatsAppNumber { get; set; }
        public string? WhatsAppMessage { get; set; }
        public bool Activo { get; set; }
        public List<LandingServiceDTOResponse> Services { get; set; } = [];
        public List<LandingGalleryItemDTOResponse> GalleryItems { get; set; } = [];
    }

    public class LandingServiceDTOResponse
    {
        public int LandingServiceId { get; set; }
        public int Orden { get; set; }
        public string IconCode { get; set; } = string.Empty;
        public string Titulo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
    }

    public class LandingGalleryItemDTOResponse
    {
        public int LandingGalleryItemId { get; set; }
        public int Orden { get; set; }
        public string MediaType { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public string? AltText { get; set; }
    }
}
