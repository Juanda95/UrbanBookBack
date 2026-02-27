using Domain.Entities.DLandingPage;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configuration.CLandingPage
{
    public class LandingServiceConfig : IEntityTypeConfiguration<LandingService>
    {
        public void Configure(EntityTypeBuilder<LandingService> builder)
        {
            builder.ToTable("LandingServices");

            builder.HasKey(ls => ls.LandingServiceId);
            builder.Property(ls => ls.LandingServiceId)
                .ValueGeneratedOnAdd();

            builder.Property(ls => ls.Orden)
                .IsRequired();

            builder.Property(ls => ls.IconCode)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(ls => ls.Titulo)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(ls => ls.Descripcion)
                .IsRequired()
                .HasMaxLength(200);

            // Multi-tenancy
            builder.Property(ls => ls.NegocioId).IsRequired();
            builder.HasOne(ls => ls.Negocio)
                .WithMany()
                .HasForeignKey(ls => ls.NegocioId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasIndex(ls => ls.NegocioId);
        }
    }
}
