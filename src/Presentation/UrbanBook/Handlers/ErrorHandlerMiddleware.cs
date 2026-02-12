using Application.Helpers.Wrappers;
using System.Net;
using System.Text.Json;

namespace UrbanBook.Handlers
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                await HandleExceptionAsync(context, error);              
            }

        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception error)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            var statusCode = HttpStatusCode.InternalServerError;
            var errorMessage = "Internal Server Error"; 
           
            switch (error)
            {
                case KeyNotFoundException e:
                    //not fount error
                    statusCode = HttpStatusCode.NotFound;
                    errorMessage = error.Message;
                    break;
                default:
                    statusCode = HttpStatusCode.InternalServerError;
                    errorMessage = error.Message;
                    break;
            }

            response.StatusCode = (int)statusCode;

            var responseModel = new Response<string>(errorMessage, statusCode);

            var result = JsonSerializer.Serialize(responseModel);
            await response.WriteAsync(result);
        }

    }
}
