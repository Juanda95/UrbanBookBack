using Domain.Entities.DNegocio;

namespace Domain.Entities.DOtp
{
    public class OtpVerification : ITenantEntity
    {
        public int OtpVerificationId { get; set; }

        public int NegocioId { get; set; }
        public Negocio? Negocio { get; set; }

        /// <summary>
        /// Número de teléfono al que se envió el OTP.
        /// </summary>
        public string Telefono { get; set; } = string.Empty;

        /// <summary>
        /// Hash SHA-256 del código de 6 dígitos.
        /// </summary>
        public string CodeHash { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
        public DateTime ExpiresAt { get; set; }

        /// <summary>
        /// Indica si el OTP ya fue utilizado.
        /// </summary>
        public bool IsUsed { get; set; } = false;

        /// <summary>
        /// Número de intentos fallidos de verificación.
        /// </summary>
        public int Attempts { get; set; } = 0;

        /// <summary>
        /// ID del cliente que solicitó el OTP (opcional).
        /// </summary>
        public int? ClienteId { get; set; }
    }
}
