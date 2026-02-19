using Domain.Entities.DCalendario;
using Domain.Entities.Dcliente;
using Domain.Entities.DMessaging;
using Domain.Entities.DServicio;
using Domain.Entities.DUsuario;
using Domain.Entities.Parametros;

namespace Domain.Entities.DNegocio
{
    /// <summary>
    /// Representa un negocio (tenant) en la plataforma multi-tenancy.
    /// Cada negocio tiene su propio subdominio: {slug}.urbanbook.nitidosport.com
    /// </summary>
    public class Negocio
    {
        public int NegocioId { get; set; }

        /// <summary>
        /// Nombre visible del negocio. Ej: "Salon Ana"
        /// </summary>
        public string Nombre { get; set; } = string.Empty;

        /// <summary>
        /// Identificador URL del negocio, usado como subdominio. Ej: "salon-ana"
        /// Solo letras minusculas, numeros y guiones.
        /// </summary>
        public string Slug { get; set; } = string.Empty;

        public string? Descripcion { get; set; }
        public string? LogoUrl { get; set; }
        public string? Telefono { get; set; }
        public string? Direccion { get; set; }
        public string? Correo { get; set; }
        public bool Activo { get; set; } = true;
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        // Navegacion
        public virtual List<Usuario> Usuarios { get; set; } = [];
        public virtual List<Cliente> Clientes { get; set; } = [];
        public virtual List<Evento> Eventos { get; set; } = [];
        public virtual List<Servicio> Servicios { get; set; } = [];
        public virtual List<SmtpConfig> SmtpConfigs { get; set; } = [];
        public virtual List<Parameter> Parameters { get; set; } = [];
        public virtual List<HorarioAtencion> HorariosAtencion { get; set; } = [];
    }
}
