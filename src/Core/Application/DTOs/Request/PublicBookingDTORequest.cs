using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Request
{
    /// <summary>
    /// DTO de solicitud para crear una reserva pública (sin autenticación).
    /// </summary>
    public class PublicBookingDTORequest
    {
        /// <summary>
        /// Identificador del cliente que realiza la reserva.
        /// </summary>
        [DisplayName("Cliente")]
        [Required(ErrorMessage = "El cliente es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID del cliente debe ser un número positivo")]
        public int ClienteId { get; set; }

        /// <summary>
        /// Identificador del profesional seleccionado.
        /// </summary>
        [DisplayName("Profesional")]
        [Required(ErrorMessage = "El profesional es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID del profesional debe ser un número positivo")]
        public int UsuarioId { get; set; }

        /// <summary>
        /// Identificador del servicio seleccionado.
        /// </summary>
        [DisplayName("Servicio")]
        [Required(ErrorMessage = "El servicio es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID del servicio debe ser un número positivo")]
        public int ServicioId { get; set; }

        /// <summary>
        /// Fecha y hora de inicio de la cita.
        /// </summary>
        [DisplayName("Fecha de Inicio")]
        [Required(ErrorMessage = "La fecha de inicio es requerida")]
        public DateTime FechaInicio { get; set; }

        /// <summary>
        /// Fecha y hora de fin de la cita.
        /// </summary>
        [DisplayName("Fecha de Fin")]
        [Required(ErrorMessage = "La fecha de fin es requerida")]
        public DateTime FechaFin { get; set; }
    }
}
