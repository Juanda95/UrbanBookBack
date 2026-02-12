namespace Application.DTOs.Request.Messaging
{
    /// <summary>
    /// Represents a request DTO for sending an email.
    /// </summary>
    public class SendEmailDTORequest
    {
        /// <summary>
        /// Gets or sets the SMTP configuration ID.
        /// </summary>
        public int SmtpConfigId { get; set; }

        /// <summary>
        /// Gets or sets the email address to send the email.
        /// </summary>
        public string EmailSend { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the subject of the email.
        /// </summary>
        public string Subject { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the body of the email.
        /// </summary>
        public string Body { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the sender's email address.
        /// </summary>
        public string From { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the recipient's email address.
        /// </summary>
        public string To { get; set; } = string.Empty;

        public Dictionary<string, string>? Placeholders { get; set; } // Para manejar reemplazos dinámicos

        // Nueva propiedad para manejar los archivos adjuntos
        public List<EmailAttachment> Attachments { get; set; } = new List<EmailAttachment>();

        public class EmailAttachment
        {
            public string FileName { get; set; } = string.Empty;
            public byte[] Content { get; set; } = Array.Empty<byte>();
            public string MimeType { get; set; } = string.Empty;
        }
    }
}
