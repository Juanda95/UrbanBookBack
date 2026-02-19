using Domain.Entities.DUsuario;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configuration.Administration
{
    public class HorarioAtencionConfig : IEntityTypeConfiguration<HorarioAtencion>
    {
        public void Configure(EntityTypeBuilder<HorarioAtencion> builder)
        {
            builder.ToTable("HorarioAtencion");

            builder.HasKey(h => h.HorarioAtencionId);
            builder.Property(h => h.HorarioAtencionId)
                .ValueGeneratedOnAdd();

            builder.Property(h => h.DiaSemana)
                .IsRequired();

            builder.Property(h => h.Activo)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(h => h.HoraInicio)
                .IsRequired()
                .HasColumnType("time");

            builder.Property(h => h.HoraFin)
                .IsRequired()
                .HasColumnType("time");

            builder.HasIndex(h => new { h.UsuarioId, h.DiaSemana })
                .IsUnique();

            builder.HasOne(h => h.Usuario)
                .WithMany(u => u.HorariosAtencion)
                .HasForeignKey(h => h.UsuarioId);

            // Multi-tenancy
            builder.Property(h => h.NegocioId).IsRequired();
            builder.HasOne(h => h.Negocio)
                .WithMany(n => n.HorariosAtencion)
                .HasForeignKey(h => h.NegocioId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasIndex(h => h.NegocioId);
        }
    }
}
