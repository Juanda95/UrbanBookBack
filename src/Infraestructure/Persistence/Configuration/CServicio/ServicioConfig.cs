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

            // Seed data con servicios iniciales
            servicio.HasData(
                new Servicio
                {
                    ServicioId = 1,
                    Nombre = "Corte de Pelo",
                    Descripcion = "Corte de pelo profesional con estilo personalizado",
                    Precio = 25000,
                    DuracionMinutos = 60,
                    Activo = true
                },
                new Servicio
                {
                    ServicioId = 2,
                    Nombre = "Corte de Pelo y Barba",
                    Descripcion = "Corte de pelo y arreglo de barba profesional",
                    Precio = 35000,
                    DuracionMinutos = 90,
                    Activo = true
                },
                new Servicio
                {
                    ServicioId = 3,
                    Nombre = "Solo Barba",
                    Descripcion = "Arreglo y perfilado de barba profesional",
                    Precio = 15000,
                    DuracionMinutos = 30,
                    Activo = true
                }
            );
        }
    }
}
