using Application.DTOs.DataEnvioNotificacion;
using Application.Services.Interfaces;
using Domain.Entities.DCalendario;
using Microsoft.Extensions.Logging;
using Persistence.UnitOfWork.Interface;
using System.Net;

namespace Application.Services.BackGroundService
{
    public class NotificationService(ILogger<NotificationService> logger, IWhatsAppService whatsAppService, IUnitOfWork unitOfWork): INotificationService
    {
        private readonly ILogger<NotificationService> _logger = logger;
        private readonly IWhatsAppService _whatsAppService = whatsAppService;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task CheckAndSendNotifications(int eventId)
        {
            try
            {
                var EventoRepositorio = _unitOfWork.GetRepository<Evento>();
                var Evento = await EventoRepositorio.FirstOrDefaultAsync(
                                            Evento => Evento.EventoId == eventId);

                DataEnvioWhatsAppDTO clientePrueba = new()
                {
                    ToPhoneNumber = "573104070385",
                    DataEmpresa = new DataEmpresaDTO()
                    {
                        Address = "Cra. 21 #4 - 06, Armenia, Quindío",
                        Latitude = "4.543175687645917",
                        Longitude = "-75.6693312131147",
                        Name = "Diego Grimaldos Caro Fisioterapeuta"
                    },
                    Parameters =
                        [
                            new()
                                {
                                    Nombre = "nombreCliente",
                                    Text = "Stefany barahona",
                                    Type = "text"
                                },
                                new()
                                {
                                    Nombre = "fechaHora",
                                    Text = "19/abr. a las 05:30 PM",
                                    Type = "text"
                                },
                                new()
                                {
                                    Nombre = "nombreEmpresa",
                                    Text = "Ft. Diego Grimaldos",
                                    Type = "text"
                                }
                        ]
                };

                var response = await _whatsAppService.SendWhatsAppMessageAsync(clientePrueba);
                if (response.HttpStatusCode.Equals(HttpStatusCode.OK))
                {
                    _logger.LogInformation($"mensaje enviado correctamente: {response.Data}");
                }
                else
                {
                    _logger.LogInformation($"Ha ocurrido un error al enviar el Mensaje: {response.Data}");
                }

            }
            catch (Exception ex)
            {

                _logger.LogError($"Excepción al enviar el mensaje: {ex.Message}");
            }
           

        }
    }
}
