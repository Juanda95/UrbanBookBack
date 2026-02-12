using System.Net;

namespace Application.Helpers.Wrappers
{
    public class Response<T>
    {

        /// <summary>
        /// Indica si la operación fue exitosa.
        /// </summary>
        public bool Succeeded { get; }

        /// <summary>
        /// Mensaje descriptivo sobre el resultado de la operación.
        /// </summary>
        public string Message { get; } = string.Empty;

        /// <summary>
        /// Lista de errores que ocurrieron durante la operación.
        /// </summary>
        public IReadOnlyList<string> Errors { get; } = new List<string>();

        /// <summary>
        /// Datos resultantes de la operación.
        /// </summary>
        public T? Data { get; }

        /// <summary>
        /// Código de estado HTTP resultante de la operación.
        /// </summary>
        public HttpStatusCode HttpStatusCode { get; } = HttpStatusCode.OK;


        /// <summary>
        /// Constructor para una respuesta exitosa con datos.
        /// </summary>
        /// <param name="data">Datos resultantes de la operación.</param>
        /// <param name="message">Mensaje descriptivo sobre el resultado de la operación.</param>
        /// <param name="httpStatusCode">Código de estado HTTP resultante de la operación.</param>
        public Response(T data, string message, HttpStatusCode httpStatusCode = HttpStatusCode.OK)
        {
            Succeeded = true;
            Message = message;
            Data = data;
            HttpStatusCode = httpStatusCode;
        }

        /// <summary>
        /// Constructor para una respuesta fallida.
        /// </summary>
        /// <param name="message">Mensaje descriptivo sobre el resultado de la operación.</param>
        /// <param name="errors">Lista de errores que ocurrieron durante la operación.</param>
        /// <param name="httpStatusCode">Código de estado HTTP resultante de la operación.</param>
        public Response(string message, List<string> errors, HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest)
        {
            Succeeded = false;
            Message = message;
            Errors = errors.AsReadOnly();
            HttpStatusCode = httpStatusCode;
        }

        /// <summary>
        /// Constructor para una respuesta fallida con un solo mensaje de error.
        /// </summary>
        /// <param name="message">Mensaje descriptivo sobre el resultado de la operación.</param>
        /// <param name="httpStatusCode">Código de estado HTTP resultante de la operación.</param>
        public Response(string message, HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest)
        {
            Succeeded = false;
            Message = message;
            HttpStatusCode = httpStatusCode;
        }

    }
}
