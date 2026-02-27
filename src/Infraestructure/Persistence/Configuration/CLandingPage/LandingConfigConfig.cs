using Domain.Entities.DLandingPage;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configuration.CLandingPage
{
    public class LandingConfigConfig : IEntityTypeConfiguration<LandingConfig>
    {
        public void Configure(EntityTypeBuilder<LandingConfig> builder)
        {
            builder.ToTable("LandingConfigs");

            builder.HasKey(lc => lc.LandingConfigId);
            builder.Property(lc => lc.LandingConfigId)
                .ValueGeneratedOnAdd();

            builder.Property(lc => lc.HeroTitle)
                .HasMaxLength(100);

            builder.Property(lc => lc.HeroSubtitle)
                .HasMaxLength(200);

            builder.Property(lc => lc.HeroImageFileName)
                .HasMaxLength(200);

            builder.Property(lc => lc.WhatsAppNumber)
                .HasMaxLength(20);

            builder.Property(lc => lc.WhatsAppMessage)
                .HasMaxLength(200);

            builder.Property(lc => lc.Activo)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(lc => lc.FechaCreacion)
                .HasColumnType("timestamp with time zone")
                .HasConversion(
                    v => v.Kind == DateTimeKind.Utc ? v : v.ToUniversalTime(),
                    v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

            builder.Property(lc => lc.FechaModificacion)
                .HasColumnType("timestamp with time zone")
                .HasConversion(
                    v => v.Kind == DateTimeKind.Utc ? v : v.ToUniversalTime(),
                    v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

            // Multi-tenancy
            builder.Property(lc => lc.NegocioId).IsRequired();
            builder.HasOne(lc => lc.Negocio)
                .WithMany(n => n.LandingConfigs)
                .HasForeignKey(lc => lc.NegocioId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasIndex(lc => lc.NegocioId);

            // Relaciones hijas
            builder.HasMany(lc => lc.LandingServices)
                .WithOne(ls => ls.LandingConfig)
                .HasForeignKey(ls => ls.LandingConfigId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(lc => lc.LandingGalleryItems)
                .WithOne(gi => gi.LandingConfig)
                .HasForeignKey(gi => gi.LandingConfigId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
