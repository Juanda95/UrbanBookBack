using Domain.Interfaces;
using Domain.Entities;
using Domain.Entities.DCalendario;
using Domain.Entities.DMessaging;
using Domain.Entities.Dcliente;
using Domain.Entities.DNegocio;
using Domain.Entities.DServicio;
using Domain.Entities.DUsuario;
using Domain.Entities.Parametros;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Persistence.Contexts
{
    public class UrbanBookDbContext : DbContext
    {
        private readonly ITenantService _tenantService;

        public UrbanBookDbContext(DbContextOptions<UrbanBookDbContext> options, ITenantService tenantService)
            : base(options)
        {
            _tenantService = tenantService;
        }

        public DbSet<Negocio> Negocios { get; set; }

        public virtual DbSet<Usuario> Usuarios { get; set; }
        public virtual DbSet<Perfil> Perfiles { get; set; }

        public DbSet<Cliente> Clientes { get; set; }

        public DbSet<Evento> Evento { get; set; }

        public DbSet<Parameter> Parameters { get; set; }
        public DbSet<Value> Values { get; set; }

        public DbSet<SmtpConfig> SmtpConfigs { get; set; }

        public DbSet<Servicio> Servicios { get; set; }

        public DbSet<HorarioAtencion> HorariosAtencion { get; set; }
        public DbSet<ExclusionHorario> ExclusionesHorario { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Aplicar configuraciones personalizadas desde el ensamblaje actual
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            // Filtros globales de tenant
            // TenantId == 0 => sin filtro (admin general o background services)
            modelBuilder.Entity<Usuario>().HasQueryFilter(
                e => _tenantService.GetCurrentTenantId() == 0 || e.NegocioId == _tenantService.GetCurrentTenantId());

            modelBuilder.Entity<Cliente>().HasQueryFilter(
                e => _tenantService.GetCurrentTenantId() == 0 || e.NegocioId == _tenantService.GetCurrentTenantId());

            modelBuilder.Entity<Evento>().HasQueryFilter(
                e => _tenantService.GetCurrentTenantId() == 0 || e.NegocioId == _tenantService.GetCurrentTenantId());

            modelBuilder.Entity<Servicio>().HasQueryFilter(
                e => _tenantService.GetCurrentTenantId() == 0 || e.NegocioId == _tenantService.GetCurrentTenantId());

            modelBuilder.Entity<SmtpConfig>().HasQueryFilter(
                e => _tenantService.GetCurrentTenantId() == 0 || e.NegocioId == _tenantService.GetCurrentTenantId());

            modelBuilder.Entity<HorarioAtencion>().HasQueryFilter(
                e => _tenantService.GetCurrentTenantId() == 0 || e.NegocioId == _tenantService.GetCurrentTenantId());

            // Parameter: NegocioId nullable - NULL = global, visible para todos los tenants
            modelBuilder.Entity<Parameter>().HasQueryFilter(
                e => _tenantService.GetCurrentTenantId() == 0 || e.NegocioId == null || e.NegocioId == _tenantService.GetCurrentTenantId());

            // Filtros matching para entidades hijas (resuelve warnings EF Core)
            // ExclusionHorario filtra a través de su padre HorarioAtencion
            modelBuilder.Entity<ExclusionHorario>().HasQueryFilter(
                e => _tenantService.GetCurrentTenantId() == 0 || e.HorarioAtencion!.NegocioId == _tenantService.GetCurrentTenantId());

            // Value filtra a través de su padre Parameter
            modelBuilder.Entity<Value>().HasQueryFilter(
                e => _tenantService.GetCurrentTenantId() == 0 || e.Parameters!.NegocioId == null || e.Parameters!.NegocioId == _tenantService.GetCurrentTenantId());
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var tenantId = _tenantService.GetCurrentTenantId();

            foreach (var entry in ChangeTracker.Entries<ITenantEntity>())
            {
                if (entry.State == EntityState.Added && tenantId > 0)
                {
                    entry.Entity.NegocioId = tenantId;
                }

                if ((entry.State == EntityState.Modified || entry.State == EntityState.Deleted)
                    && tenantId > 0
                    && entry.Entity.NegocioId != tenantId)
                {
                    throw new UnauthorizedAccessException("No tiene permiso para modificar datos de otro negocio.");
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
