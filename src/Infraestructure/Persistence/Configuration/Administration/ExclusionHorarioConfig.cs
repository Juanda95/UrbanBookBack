using Domain.Entities.DUsuario;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configuration.Administration
{
    public class ExclusionHorarioConfig : IEntityTypeConfiguration<ExclusionHorario>
    {
        public void Configure(EntityTypeBuilder<ExclusionHorario> builder)
        {
            builder.ToTable("ExclusionHorario");

            builder.HasKey(e => e.ExclusionHorarioId);
            builder.Property(e => e.ExclusionHorarioId)
                .ValueGeneratedOnAdd();

            builder.Property(e => e.HoraInicio)
                .IsRequired()
                .HasColumnType("time");

            builder.Property(e => e.HoraFin)
                .IsRequired()
                .HasColumnType("time");

            builder.Property(e => e.Descripcion)
                .HasMaxLength(200);

            builder.HasOne(e => e.HorarioAtencion)
                .WithMany(h => h.Exclusiones)
                .HasForeignKey(e => e.HorarioAtencionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
