using Application.DTOs.Request.Messaging;
using Application.Helpers.Wrappers;
using Domain.Entities.DMessaging;

namespace Application.Services.Interfaces.Messaging
{
    public interface IEmailService
    {
        Task<Response<bool>> SendEmailAsync(SendEmailDTORequest EmailRequest);
       
    }
}
