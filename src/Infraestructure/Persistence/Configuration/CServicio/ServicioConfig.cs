using Domain.Entities.DServicio;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configuration.CServicio
{
    public class ServicioConfig : IEntityTypeConfiguration<Servicio>
    {
        public void Configure(EntityTypeBuilder<Servicio> servicio)
        {
            servicio.ToTable("Servicio");

            servicio.HasKey(s => s.ServicioId);

            servicio.Property(s => s.ServicioId)
                .ValueGeneratedOnAdd();

            servicio.Property(s => s.Nombre)
                .IsRequired()
                .HasMaxLength(100);

            servicio.Property(s => s.Descripcion)
                .HasMaxLength(500);

            servicio.Property(s => s.Precio)
                .IsRequired()
                .HasColumnType("decimal(10,2)");

            servicio.Property(s => s.DuracionMinutos)
                .IsRequired();

            servicio.Property(s => s.ImagenUrl)
                .HasMaxLength(500);

            servicio.Property(s => s.Activo)
                .IsRequired()
                .HasDefaultValue(true);

            // Multi-tenancy (seed data removida - se crean servicios al crear negocio)
            servicio.Property(s => s.NegocioId).IsRequired();
            servicio.HasOne(s => s.Negocio)
                .WithMany(n => n.Servicios)
                .HasForeignKey(s => s.NegocioId)
                .OnDelete(DeleteBehavior.Restrict);
            servicio.HasIndex(s => s.NegocioId);
        }
    }
}
