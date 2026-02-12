using Application.Helpers.Wrappers;
using System.Net;
using System.Text.Json;

namespace UrbanBook.Handlers
{
    public class ValidationHandlerMiddleware(ILogger<ValidationHandlerMiddleware> logger) : IMiddleware
    {
        private readonly ILogger<ValidationHandlerMiddleware> _logger = logger;
        private const int BadRequestStatusCode = StatusCodes.Status400BadRequest;
        private const int StatusCodeNotFound = StatusCodes.Status404NotFound;
        private const int Unauthorized = StatusCodes.Status401Unauthorized;
        private const string JsonContentType = "application/json";
        private const string ProblemJsonContentType = "application/problem+json";

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                var originalResponseBodyStream = context.Response.Body;

                using (var responseBodyStream = new MemoryStream())
                {
                    context.Response.Body = responseBodyStream;

                    await next(context);

                    context.Response.Body = originalResponseBodyStream;
                    responseBodyStream.Seek(0, SeekOrigin.Begin);
                    var responseBody = await new StreamReader(responseBodyStream).ReadToEndAsync();

                    if (context.Response.StatusCode == BadRequestStatusCode &&
                        context.Response.ContentType != null &&
                        (context.Response.ContentType.Equals(JsonContentType, StringComparison.OrdinalIgnoreCase) ||
                         context.Response.ContentType.Contains(ProblemJsonContentType, StringComparison.OrdinalIgnoreCase)))
                    {
                        try
                        {
                            var errorMessages = ExtractErrorMessages(responseBody);

                            if (errorMessages.Count > 0)
                            {
                                ResponseContext(context, errorMessages, "Los campos son obligatorios", HttpStatusCode.BadRequest);
                                return;
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, $"ocurrio un error en el validador de estado {BadRequestStatusCode}");
                        }
                    }
                    else if (context.Response.StatusCode == StatusCodeNotFound)
                    {
                        try
                        {
                            var endpoint = context.GetEndpoint();

                            if (endpoint == null)
                            {
                                ResponseContext(context, new List<string>(), "La pagina no existe", HttpStatusCode.NotFound);
                                return;
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, $"ocurrio un error en el validador de estado {StatusCodeNotFound}");
                        }

                    }
                    else if (context.Response.StatusCode == Unauthorized)
                    {
                        try
                        {

                            ResponseContext(context, new List<string>(), "Usted no esta autorizado para realizar esta peticion", HttpStatusCode.Unauthorized);
                            return;

                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, $"ocurrio un error en el validador de estado {Unauthorized}");
                        }

                    }

                    // Si no es un error 400, copiar el cuerpo original de nuevo
                    responseBodyStream.Seek(0, SeekOrigin.Begin);
                    await responseBodyStream.CopyToAsync(originalResponseBodyStream);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred while processing the request");
                throw;
            }
        }
        private async void ResponseContext(HttpContext context, List<string> errorMessages, string Message, HttpStatusCode statuscode)
        {
            var response = new Response<string>(Message, errorMessages, statuscode);
            var jsonResponse = JsonSerializer.Serialize(response);

            context.Response.ContentType = JsonContentType;
            context.Response.ContentLength = jsonResponse.Length;

            await context.Response.WriteAsync(jsonResponse);
        }

        private List<string> ExtractErrorMessages(string responseBody)
        {
            var messages = new List<string>();
            using (JsonDocument doc = JsonDocument.Parse(responseBody))
            {
                var root = doc.RootElement;
                if (root.TryGetProperty("errors", out JsonElement errorsElement) && errorsElement.ValueKind == JsonValueKind.Object)
                {
                    foreach (var property in errorsElement.EnumerateObject())
                    {
                        if (property.Value.ValueKind == JsonValueKind.Array)
                        {
                            foreach (var messageElement in property.Value.EnumerateArray())
                            {
                                var message = messageElement.GetString();
                                if (message != null)
                                {
                                    messages.Add(message);
                                }
                            }
                        }
                    }
                }
            }
            return messages;
        }
    }


}


