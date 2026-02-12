using Application.DTOs.DataEnvioNotificacion;
using Application.Helpers.Wrappers;
using Application.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace ExternalServices.Services
{
    public class WhatsAppService(IHttpClientFactory httpClientFactory,IConfiguration configuration, ILogger<WhatsAppService> logger, IUtils util) : IWhatsAppService
    {
        private readonly string _authToken = configuration["WhatsApp:AuthToken"] ?? string.Empty;
        private readonly IUtils _Util = util;
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
        private readonly ILogger<WhatsAppService> _logger = logger;

        
        public async Task<Response<string>> SendWhatsAppMessageAsync(DataEnvioWhatsAppDTO dataEnvio)
        {
           
            var client = _httpClientFactory.CreateClient("WhatsAppApi");
            var message = _Util.TemplateRecordatorio(dataEnvio);
            var byteData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

            using var content = new ByteArrayContent(byteData);
            string phoneNumberId = "369414622912991" ?? string.Empty;
            string uri = $"/v20.0/{phoneNumberId}/messages" ?? string.Empty;

            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _authToken);

            //var response = await client.PostAsync(uri, content);
            //var responseContent = await response.Content.ReadAsStringAsync();
            //if (response.IsSuccessStatusCode)
            //{            
            //    _logger.LogInformation($"Sent WhatsApp message to {""}");
            //    return new Response<string>(responseContent, "Evento enviado correctamente");
            //}
            //else
            //{
            //    _logger.LogError($"Failed to send WhatsApp message to {""}. Status Code: {response.StatusCode}");
            //    return new Response<string>(responseContent, "Error en el envio del evento", response.StatusCode);
            //}
            return new Response<string>("Prueba", "Error en el envio del evento");
        }

    }
}
