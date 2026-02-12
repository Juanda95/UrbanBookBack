using Domain.Entities.DCalendario;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configuration.CCalendario
{
    public class StateProcessEventConfig: IEntityTypeConfiguration<StateProcessEvents>
    {
        public void Configure(EntityTypeBuilder<StateProcessEvents> StateProcessEvent)
        {
            StateProcessEvent.ToTable("StateProcessEvent");

            StateProcessEvent.HasKey(s => s.StateProcessEventsId);

            StateProcessEvent.Property(s => s.StateProcessEventsId)
                .ValueGeneratedOnAdd();

            StateProcessEvent.Property(s => s.State)
                .IsRequired()
                .HasMaxLength(50);

            StateProcessEvent.HasMany(s => s.Eventos)
                .WithOne(e => e.StateProcessEvent)
                .HasForeignKey(e => e.StateProcessEventId);
        }

    }
}
