namespace Application.DTOs.Response
{
    public class UsuarioDTOResponse
    {
        /// <summary>
        /// Identificador único del usuario.
        /// </summary>
        public int UsuarioId { get; set; } 

        /// <summary>
        /// Nombre de usuario o alias.
        /// </summary>
        public string Username { get; set; } = string.Empty;

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
        /// Correo Electronico del usuario
        /// </summary>
        public string Correo { get; set; } = string.Empty;

        /// <summary>
        /// Lista de perfiles asociados al usuario.
        /// Los perfiles definen roles o grupos a los que el usuario pertenece.
        /// </summary>
        public List<PerfilDTOResponse> Perfiles { get; set; } = [];


    }
}
