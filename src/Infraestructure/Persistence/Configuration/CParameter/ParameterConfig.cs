using Domain.Entities.Parametros;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configuration.CParameter
{
    public class ParameterConfig : IEntityTypeConfiguration<Parameter>
    {
        public void Configure(EntityTypeBuilder<Parameter> Parameter)
        {
            Parameter.ToTable("Parameters");

            Parameter.HasKey(p => p.IdParameter);
            Parameter.Property(p => p.IdParameter)
                .ValueGeneratedOnAdd();

            Parameter.Property(p => p.TypeParameter)
                .IsRequired()
                .HasMaxLength(100);

            Parameter.Property(p => p.CreationDate)
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("now()")
                .HasConversion(
                    v => v.Kind == DateTimeKind.Utc ? v : v.ToUniversalTime(),
                    v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

            Parameter.Property(p => p.ModifierDate)
                .IsRequired()
                .HasColumnType("timestamp with time zone")
                .HasConversion(
                    v => v.Kind == DateTimeKind.Utc ? v : v.ToUniversalTime(),
                    v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

            Parameter.Property(p => p.CreationUser)
                .IsRequired()
                .HasMaxLength(100);

            Parameter.Property(p => p.ModifierUser)
                .IsRequired()
                .HasMaxLength(100);

            Parameter.HasMany(p => p.Values)
                .WithOne(v => v.Parameters)
                .HasForeignKey(v => v.IdParameter);

            // Multi-tenancy (NegocioId nullable: NULL = parameter global del sistema)
            Parameter.Property(p => p.NegocioId).IsRequired(false);
            Parameter.HasOne(p => p.Negocio)
                .WithMany(n => n.Parameters)
                .HasForeignKey(p => p.NegocioId)
                .OnDelete(DeleteBehavior.Restrict);
            Parameter.HasIndex(p => p.NegocioId);
        }
    }
}
