namespace Application.DTOs.Response
{
    public class NegocioDTOResponse
    {
        public int NegocioId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public string? LogoUrl { get; set; }
        public string? Telefono { get; set; }
        public string? Direccion { get; set; }
        public string? Correo { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int TotalUsuarios { get; set; }
        public int TotalClientes { get; set; }
    }

    /// <summary>
    /// DTO ligero para endpoint publico /api/tenant/info
    /// </summary>
    public class TenantInfoDTOResponse
    {
        public string Nombre { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public string? LogoUrl { get; set; }
        public string? Telefono { get; set; }
        public string? Direccion { get; set; }
        public string? Correo { get; set; }
    }
}
