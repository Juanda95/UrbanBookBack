using Domain.Entities.Dcliente;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configuration.Ccliente
{
    public class ClienteConfig : IEntityTypeConfiguration<Cliente>
    {
        public void Configure(EntityTypeBuilder<Cliente> cliente)
        {
            cliente.ToTable("cliente");

            cliente.HasKey(p => p.clienteId);

            cliente.Property(p => p.clienteId)
                .ValueGeneratedOnAdd();

            cliente.Property(p => p.Nombres)
                .IsRequired()
                .HasMaxLength(100);

            cliente.Property(p => p.PrimerApellido)
                .IsRequired()
                .HasMaxLength(100);

            cliente.Property(p => p.SegundoApellido)
                .IsRequired()
                .HasMaxLength(100);

            cliente.Property(p => p.NumeroDocumento)
                .IsRequired(false)
                .HasMaxLength(100);

            cliente.Property(p => p.Telefono)
                .IsRequired()
                .HasMaxLength(100);

            cliente.Property(p => p.Correo)
                .IsRequired()
                .HasMaxLength(100);

            cliente.HasMany(p => p.Eventos)
                .WithOne(c => c.Cliente)
                .HasForeignKey(c => c.ClienteId)
                .OnDelete(DeleteBehavior.Cascade);

            // Multi-tenancy
            cliente.Property(c => c.NegocioId).IsRequired();
            cliente.HasOne(c => c.Negocio)
                .WithMany(n => n.Clientes)
                .HasForeignKey(c => c.NegocioId)
                .OnDelete(DeleteBehavior.Restrict);
            cliente.HasIndex(c => c.NegocioId);
        }
    }
}
