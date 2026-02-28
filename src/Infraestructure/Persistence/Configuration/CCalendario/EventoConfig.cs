using Domain.Entities.DCalendario;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configuration.CCalendario
{
    internal class EventoConfig : IEntityTypeConfiguration<Evento>
    {
        public void Configure(EntityTypeBuilder<Evento> Evento)
        {
            Evento.ToTable("Evento");

            Evento.HasKey(e => e.EventoId);

            Evento.Property(e => e.Titulo)
                .IsRequired()
                .HasMaxLength(100);

            Evento.Property(e => e.Descripcion)
                .HasMaxLength(1000);

            Evento.Property(e => e.Estado)
                .IsRequired();

            Evento.Property(e => e.FechaInicio)
                .IsRequired()
                .HasColumnType("timestamp with time zone")
                .HasConversion(
                    v => v.Kind == DateTimeKind.Utc ? v : v.ToUniversalTime(),
                    v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

            Evento.Property(e => e.FechaFin)
                .IsRequired()
                .HasColumnType("timestamp with time zone")
                .HasConversion(
                    v => v.Kind == DateTimeKind.Utc ? v : v.ToUniversalTime(),
                    v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

            Evento.Property(e => e.FechaRegistro)
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("now()")
                .IsRequired()
                .HasConversion(
                    v => v.Kind == DateTimeKind.Utc ? v : v.ToUniversalTime(),
                    v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

            Evento.Property(e => e.IsScheduled)
                .IsRequired()
                .HasDefaultValue(false);

            Evento.Property(e => e.Motivo)
                .HasMaxLength(500);

            Evento.HasIndex(e => e.FechaInicio);

            Evento.HasIndex(e => e.IsScheduled);

            Evento.HasOne(c => c.Cliente)
                .WithMany(p => p.Eventos)
                .HasForeignKey(c => c.ClienteId);

            Evento.HasOne(c => c.Usuario)
                .WithMany(p => p.Eventos)
                .HasForeignKey(c => c.UsuarioId);

            Evento.HasOne(e => e.StateProcessEvent)
                .WithMany(s => s.Eventos)
                .HasForeignKey(e => e.StateProcessEventId);

            Evento.Property(e => e.Precio)
                .HasColumnType("numeric(18,2)")
                .IsRequired(false);

            Evento.Property(e => e.ServicioId)
                .IsRequired(false);

            Evento.HasOne(e => e.Servicio)
                .WithMany()
                .HasForeignKey(e => e.ServicioId)
                .OnDelete(DeleteBehavior.SetNull);

            Evento.HasIndex(e => e.ServicioId);

            // Multi-tenancy
            Evento.Property(e => e.NegocioId).IsRequired();
            Evento.HasOne(e => e.Negocio)
                .WithMany(n => n.Eventos)
                .HasForeignKey(e => e.NegocioId)
                .OnDelete(DeleteBehavior.Restrict);
            Evento.HasIndex(e => e.NegocioId);

        }
    }
}
