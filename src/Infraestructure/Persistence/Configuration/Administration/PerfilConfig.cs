using Domain.Entities.DUsuario;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configuration.Administration
{
    public class PerfilConfig : IEntityTypeConfiguration<Perfil>
    {
        public void Configure(EntityTypeBuilder<Perfil> Perfil)
        {
            Perfil.ToTable("Perfil");

            Perfil.HasKey(p => p.PerfilId);
            Perfil.Property(d => d.PerfilId)
            .ValueGeneratedOnAdd();

            Perfil.Property(p => p.Nombre)
                .IsRequired() 
                .HasMaxLength(100);

            Perfil.Property(p => p.Descripcion)
                .HasMaxLength(500);

            Perfil.Property(p => p.Rol);

            Perfil.HasMany(p => p.Usuarios) 
                .WithMany(u => u.Perfiles) 
                .UsingEntity(j =>
                {
                    j.ToTable("UsuarioPerfil");
                });
        }
    }
}
