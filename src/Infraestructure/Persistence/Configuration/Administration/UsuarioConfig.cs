using Domain.Entities.Dcliente;
using Domain.Entities.DUsuario;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configuration.Administration
{
    public class UsuarioConfig : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> Usuario)
        {
            Usuario.ToTable("Usuario");

            Usuario.HasKey(u => u.UsuarioId);
            Usuario.Property(d => d.UsuarioId)
            .ValueGeneratedOnAdd();

            Usuario.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(150);

            Usuario.Property(u => u.Password)
                .IsRequired()
                .HasMaxLength(300);

            Usuario.Property(u => u.Nombre)
                .IsRequired()
                .HasMaxLength(150);

            Usuario.Property(u => u.Apellido)
                .IsRequired()
                .HasMaxLength(150);

            Usuario.Property(u => u.Direccion)
                .IsRequired()
                .HasMaxLength(150);

            Usuario.Property(u => u.Telefono)
                .IsRequired()
                .HasMaxLength(150);

            Usuario.HasMany(u => u.Perfiles)
                .WithMany(p => p.Usuarios)
                .UsingEntity(j =>
                {
                    j.ToTable("UsuarioPerfil");
                });

            Usuario.HasMany(p => p.Eventos)
                .WithOne(c => c.Usuario)
                .HasForeignKey(c => c.UsuarioId);

            // Multi-tenancy
            Usuario.Property(u => u.NegocioId).IsRequired();
            Usuario.HasOne(u => u.Negocio)
                .WithMany(n => n.Usuarios)
                .HasForeignKey(u => u.NegocioId)
                .OnDelete(DeleteBehavior.Restrict);
            Usuario.HasIndex(u => u.NegocioId);

        }
    }
}
