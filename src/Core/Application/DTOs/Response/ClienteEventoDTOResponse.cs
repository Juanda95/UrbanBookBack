namespace Application.DTOs.Response
{
    /// <summary>
    /// DTO de cliente para contexto de Eventos.
    /// Incluye solo información no sensible necesaria para mostrar datos básicos del cliente en citas.
    /// Este DTO está diseñado para proyecciones eficientes en consultas.
    /// </summary>
    public class ClienteEventoDTOResponse
    {
        /// <summary>
        /// Identificador único del cliente.
        /// </summary>
        public int ClienteId { get; set; }

        /// <summary>
        /// Nombre del cliente.
        /// </summary>
        public string Nombres { get; set; } = string.Empty;

        /// <summary>
        /// Primer apellido del cliente.
        /// </summary>
        public string PrimerApellido { get; set; } = string.Empty;

        /// <summary>
        /// Segundo apellido del cliente.
        /// </summary>
        public string SegundoApellido { get; set; } = string.Empty;

        /// <summary>
        /// Tipo de documento del cliente (CC, TI, Pasaporte, etc.).
        /// </summary>
        public string TipoDocumento { get; set; } = string.Empty;

        /// <summary>
        /// Número de documento del cliente.
        /// </summary>
        public string NumeroDocumento { get; set; } = string.Empty;

        /// <summary>
        /// Correo de contacto del cliente.
        /// </summary>
        public string Correo { get; set; } = string.Empty;

        /// <summary>
        /// Teléfono de contacto del cliente.
        /// </summary>
        public string Telefono { get; set; } = string.Empty;

        /// <summary>
        /// Fecha de nacimiento del cliente (para cálculo de edad).
        /// </summary>
        public DateTime FechaNacimiento { get; set; }
    }
}
