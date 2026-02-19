using Domain.Entities.DNegocio;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configuration.CNegocio
{
    public class NegocioConfig : IEntityTypeConfiguration<Negocio>
    {
        public void Configure(EntityTypeBuilder<Negocio> builder)
        {
            builder.ToTable("Negocio");

            builder.HasKey(n => n.NegocioId);
            builder.Property(n => n.NegocioId)
                .ValueGeneratedOnAdd();

            builder.Property(n => n.Nombre)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(n => n.Slug)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasIndex(n => n.Slug)
                .IsUnique();

            builder.Property(n => n.Descripcion)
                .HasMaxLength(500);

            builder.Property(n => n.LogoUrl)
                .HasMaxLength(500);

            builder.Property(n => n.Telefono)
                .HasMaxLength(50);

            builder.Property(n => n.Direccion)
                .HasMaxLength(300);

            builder.Property(n => n.Correo)
                .HasMaxLength(150);

            builder.Property(n => n.Activo)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(n => n.FechaCreacion)
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("now()")
                .HasConversion(
                    v => v.Kind == DateTimeKind.Utc ? v : v.ToUniversalTime(),
                    v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
        }
    }
}
