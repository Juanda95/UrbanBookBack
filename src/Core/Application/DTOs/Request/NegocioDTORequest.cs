using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Request
{
    public class NegocioDTORequest
    {
        [Required, MaxLength(200)]
        public string Nombre { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string Slug { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Descripcion { get; set; }

        [MaxLength(500)]
        public string? LogoUrl { get; set; }

        [MaxLength(50)]
        public string? Telefono { get; set; }

        [MaxLength(300)]
        public string? Direccion { get; set; }

        [MaxLength(150)]
        public string? Correo { get; set; }

        // Datos del usuario administrador inicial (solo para creación)
        [MaxLength(100)]
        public string? AdminEmail { get; set; }

        [MaxLength(50)]
        public string? AdminPassword { get; set; }

        [MaxLength(50)]
        public string? AdminNombre { get; set; }

        [MaxLength(50)]
        public string? AdminApellido { get; set; }
    }

    public class NegocioDTOUpdateRequest : NegocioDTORequest
    {
        [Required]
        public int NegocioId { get; set; }
        public bool Activo { get; set; } = true;
    }
}
