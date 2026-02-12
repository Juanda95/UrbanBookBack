namespace Application.DTOs.Response
{
    public class PerfilDTOResponse
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
        /// identificador del perfil
        /// </summary>
        public string Rol { get; set; } = string.Empty;

    }
}
