namespace Application.DTOs.Response
{
    public class ClienteDTOResponse
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
        /// Número de documento del cliente.
        /// </summary>
        public string NumeroDocumento { get; set; } = string.Empty;

        /// <summary>
        /// Teléfono del cliente.
        /// </summary>
        public string Telefono { get; set; } = string.Empty;

        /// <summary>
        /// Correo del cliente.
        /// </summary>
        public string Correo { get; set; } = string.Empty;
    }
}
