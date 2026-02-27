using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Request
{
    public class LandingConfigDTORequest
    {
        public int? LandingConfigId { get; set; }

        [DisplayName("Titulo Hero")]
        [MaxLength(100, ErrorMessage = "El titulo debe tener menos de 100 caracteres")]
        public string? HeroTitle { get; set; }

        [DisplayName("Subtitulo Hero")]
        [MaxLength(200, ErrorMessage = "El subtitulo debe tener menos de 200 caracteres")]
        public string? HeroSubtitle { get; set; }

        [MaxLength(200, ErrorMessage = "El nombre del archivo debe tener menos de 200 caracteres")]
        public string? HeroImageFileName { get; set; }

        [MaxLength(20, ErrorMessage = "El numero de WhatsApp debe tener menos de 20 caracteres")]
        public string? WhatsAppNumber { get; set; }

        [MaxLength(200, ErrorMessage = "El mensaje de WhatsApp debe tener menos de 200 caracteres")]
        public string? WhatsAppMessage { get; set; }

        public List<LandingServiceDTORequest> Services { get; set; } = [];
        public List<LandingGalleryItemDTORequest> GalleryItems { get; set; } = [];
    }

    public class LandingServiceDTORequest
    {
        public int? LandingServiceId { get; set; }

        [Required(ErrorMessage = "El orden es requerido")]
        public int Orden { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "El icono es requerido")]
        [MaxLength(50, ErrorMessage = "El codigo del icono debe tener menos de 50 caracteres")]
        public string IconCode { get; set; } = string.Empty;

        [Required(AllowEmptyStrings = false, ErrorMessage = "El titulo es requerido")]
        [MaxLength(100, ErrorMessage = "El titulo debe tener menos de 100 caracteres")]
        public string Titulo { get; set; } = string.Empty;

        [Required(AllowEmptyStrings = false, ErrorMessage = "La descripcion es requerida")]
        [MaxLength(200, ErrorMessage = "La descripcion debe tener menos de 200 caracteres")]
        public string Descripcion { get; set; } = string.Empty;
    }

    public class LandingGalleryItemDTORequest
    {
        public int? LandingGalleryItemId { get; set; }

        [Required(ErrorMessage = "El orden es requerido")]
        public int Orden { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "El tipo de media es requerido")]
        [MaxLength(10)]
        public string MediaType { get; set; } = "image";

        [Required(AllowEmptyStrings = false, ErrorMessage = "El nombre del archivo es requerido")]
        [MaxLength(200)]
        public string FileName { get; set; } = string.Empty;

        [MaxLength(200)]
        public string? AltText { get; set; }
    }
}
