using Domain.Entities.DMessaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configuration.CMessaging
{
    public class SmtpConfigConfig : IEntityTypeConfiguration<SmtpConfig>
    {
        public void Configure(EntityTypeBuilder<SmtpConfig> smtpConfig)
        {
            smtpConfig.ToTable("SmtpConfigs");

            smtpConfig.HasKey(s => s.SmtpConfigId);

            smtpConfig.Property(s => s.SmtpConfigId)
                .ValueGeneratedOnAdd();

            smtpConfig.Property(s => s.Host)
                .HasMaxLength(100)
                .IsRequired();

            smtpConfig.Property(s => s.Port)
                .IsRequired();

            smtpConfig.Property(s => s.EnableSSL)
                .IsRequired();

            smtpConfig.Property(s => s.Username)
                .HasMaxLength(100)
                .IsRequired();

            smtpConfig.Property(s => s.Password)
                .HasMaxLength(100)
                .IsRequired();
            
            
        }
    }
}
