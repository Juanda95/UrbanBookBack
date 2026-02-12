using Domain.Entities.DCalendario;
using Domain.Entities.DMessaging;
using Domain.Entities.Dcliente;
using Domain.Entities.DServicio;
using Domain.Entities.DUsuario;
using Domain.Entities.Parametros;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Persistence.Contexts
{
    public class UrbanBookDbContext(DbContextOptions<UrbanBookDbContext> options) : DbContext(options)
    {

        public virtual DbSet<Usuario> Usuarios { get; set; }
        public virtual DbSet<Perfil> Perfiles { get; set; }

        public DbSet<Cliente> Clientes { get; set; }

        public DbSet<Evento> Evento { get; set; }

        public DbSet<Parameter> Parameters { get; set; }
        public DbSet<Value> Values { get; set; }

        public DbSet<SmtpConfig> SmtpConfigs { get; set; }

        public DbSet<Servicio> Servicios { get; set; }
         
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Aplicar configuraciones personalizadas desde el ensamblaje actual
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
        
    }
}
