using Domain.Entities.DCalendario;
using Domain.Entities.DNegocio;

namespace Domain.Entities.Dcliente
{
    /// <summary>
    /// Representa un cliente en el sistema.
    /// Contiene información básica del cliente: nombre, apellidos, número de documento, teléfono y correo.
    /// </summary>
    public class Cliente : ITenantEntity
    {
        /// <summary>
        /// Identificador del negocio (tenant) al que pertenece este cliente.
        /// </summary>
        public int NegocioId { get; set; }
        public Negocio? Negocio { get; set; }

        /// <summary>
        /// Identificador único del cliente.
        /// </summary>
        public int clienteId { get; set; }

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
        /// Correo electrónico del cliente.
        /// </summary>
        public string Correo { get; set; } = string.Empty;

        /// <summary>
        /// Lista de eventos asociados al cliente.
        /// </summary>
        public virtual List<Evento> Eventos { get; set; } = new List<Evento>();
    }
}
