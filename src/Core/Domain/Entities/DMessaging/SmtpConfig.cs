using Domain.Entities.DNegocio;

namespace Domain.Entities.DMessaging
{
    /// <summary>
    /// Represents the SMTP configuration settings.
    /// </summary>
    public class SmtpConfig : ITenantEntity
    {
        /// <summary>
        /// Identificador del negocio (tenant) al que pertenece esta configuracion SMTP.
        /// </summary>
        public int NegocioId { get; set; }
        public Negocio? Negocio { get; set; }

        /// <summary>
        /// Gets or sets the ID of the SMTP configuration.
        /// </summary>
        public int SmtpConfigId { get; set; }
         
        /// <summary>
        /// Gets or sets the SMTP server host.
        /// </summary>
        public string Host { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the SMTP server port.
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to enable SSL for the SMTP connection.
        /// </summary>
        public bool EnableSSL { get; set; }

        /// <summary>
        /// Gets or sets the username for the SMTP server.
        /// </summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the password for the SMTP server.
        /// </summary>
        public string Password { get; set; } = string.Empty;
    }
}
