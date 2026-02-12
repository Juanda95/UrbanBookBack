using Application.DTOs.DataEnvioNotificacion;
using Application.Helpers.Wrappers;

namespace Application.Services.Interfaces
{
    public interface IWhatsAppService
    {
        Task<Response<string>> SendWhatsAppMessageAsync(DataEnvioWhatsAppDTO dataEnvio);
    }
}
