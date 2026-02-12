using Domain.Entities.Parametros;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configuration.CParameter
{
    public class ValueConfig : IEntityTypeConfiguration<Value>
    {

        public void Configure(EntityTypeBuilder<Value> Value)
        {
            Value.ToTable("Values");

            Value.HasKey(v => v.IdValue);
            Value.Property(v => v.IdValue)
                .ValueGeneratedOnAdd();

            Value.Property(v => v.Code)
                .IsRequired()
                .HasMaxLength(50);

            Value.Property(v => v.Name)
                .IsRequired()
                .HasMaxLength(100);

            Value.Property(v => v.Description)
                .IsRequired()
                .HasMaxLength(200);

            Value.HasOne(v => v.Parameters)
                .WithMany(p => p.Values)
                .HasForeignKey(v => v.IdParameter);
        }

    }
}
