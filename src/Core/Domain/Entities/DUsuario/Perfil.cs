namespace Domain.Entities.DUsuario
{
    /// <summary>
    /// Representa un perfil en el sistema.
    /// Un perfil puede tener un nombre y una descripción, y está asociado con múltiples usuarios.
    /// </summary>
    public class Perfil
    {
        /// <summary>
        /// Identificador único para el perfil.
        /// </summary>
        public int PerfilId { get; set; }

        /// <summary>
        /// Nombre del perfil.
        /// Puede ser utilizado para identificar el tipo de perfil, como "Administrador" o "Usuario".
        /// </summary>
        public string Nombre { get; set; } = string.Empty;

        /// <summary>
        /// Rol .
        /// Puede ser utilizado para identificar el tipo de perfil.
        /// </summary>
        public string Rol { get; set; } = string.Empty;

        /// <summary>
        /// Descripción del perfil.
        /// Proporciona información adicional sobre el propósito o alcance del perfil.
        /// </summary>
        public string Descripcion { get; set; } = string.Empty;

        /// <summary>
        /// Colección de usuarios asociados con este perfil.
        /// Una relación de muchos a muchos, donde un perfil puede estar asociado con múltiples usuarios.
        /// </summary>
        public virtual List<Usuario> Usuarios { get; set; } = new List<Usuario>();
    }
}
