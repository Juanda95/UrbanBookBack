using Domain.Entities.DCalendario;

namespace Domain.Entities.DUsuario
{
    /// <summary>
    /// Representa un usuario del sistema.
    /// Contiene información básica como el ID, el nombre de usuario y la contraseña, así como una lista de perfiles asociados.
    /// </summary>
    public class Usuario
    {
        /// <summary>
        /// Identificador único del usuario.
        /// </summary>
        public int UsuarioId { get; set; }

        /// <summary>
        /// Correo electronico del usuario
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Contraseña del usuario.
        /// Nota: Debe ser manejada con cuidado y, si es posible, almacenada en formato cifrado o hash.
        /// </summary>
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// Nombre del usuario.
        /// </summary>
        public string Nombre { get; set; } = string.Empty;

        /// <summary>
        /// Apellido del usuario.
        /// </summary>
        public string Apellido { get; set; } = string.Empty;

        /// <summary>
        /// Dirección del usuario.
        /// </summary>
        public string Direccion { get; set; } = string.Empty;

        /// <summary>
        /// Teléfono del usuario.
        /// </summary>
        public string Telefono { get; set; } = string.Empty;

        /// <summary>
        /// Lista de perfiles asociados al usuario.
        /// Los perfiles definen roles o grupos a los que el usuario pertenece.
        /// </summary>
        public virtual List<Perfil> Perfiles { get; set; } = [];

        /// <summary>
        /// Lista de eventos asociados al usuario.
        /// </summary>
        public virtual List<Evento> Eventos { get; set; } = [];

        /// <summary>
        /// Lista de horarios de atención configurados por día de la semana.
        /// </summary>
        public virtual List<HorarioAtencion> HorariosAtencion { get; set; } = [];

    }
}
