using Domain.Entities.DLandingPage;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configuration.CLandingPage
{
    public class LandingGalleryItemConfig : IEntityTypeConfiguration<LandingGalleryItem>
    {
        public void Configure(EntityTypeBuilder<LandingGalleryItem> builder)
        {
            builder.ToTable("LandingGalleryItems");

            builder.HasKey(gi => gi.LandingGalleryItemId);
            builder.Property(gi => gi.LandingGalleryItemId)
                .ValueGeneratedOnAdd();

            builder.Property(gi => gi.Orden)
                .IsRequired();

            builder.Property(gi => gi.MediaType)
                .IsRequired()
                .HasMaxLength(10);

            builder.Property(gi => gi.FileName)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(gi => gi.AltText)
                .HasMaxLength(200);

            // Multi-tenancy
            builder.Property(gi => gi.NegocioId).IsRequired();
            builder.HasOne(gi => gi.Negocio)
                .WithMany()
                .HasForeignKey(gi => gi.NegocioId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasIndex(gi => gi.NegocioId);
        }
    }
}
