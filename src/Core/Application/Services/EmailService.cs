using Application.DTOs.Request.Messaging;
using Application.Helpers.Wrappers;
using Application.Services.Interfaces.Messaging;
using Domain.Entities.DMessaging;
using Persistence.UnitOfWork.Interface;
using System.Net;
using System.Net.Mail;

namespace Application.Services
{
    public class EmailService(IUnitOfWork unitOfWork) : IEmailService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Response<bool>> SendEmailAsync(SendEmailDTORequest emailRequest)
        {
            try
            {
                var SmtpRepositorio = _unitOfWork.GetRepository<SmtpConfig>();
                var SmtpConfig = await SmtpRepositorio.FirstOrDefaultAsync(x => x.SmtpConfigId == emailRequest.SmtpConfigId);
                if (SmtpConfig == null)
                {
                    return new Response<bool>("SMTP configuration not found.", System.Net.HttpStatusCode.NotFound);
                }

                using var smtpClient = new SmtpClient(SmtpConfig.Host)
                {
                    Port = SmtpConfig.Port,
                    Credentials = new NetworkCredential(SmtpConfig.Username, SmtpConfig.Password),
                    EnableSsl = SmtpConfig.EnableSSL
                };

                // Reemplazar los placeholders en el cuerpo del correo
                var body = ReplacePlaceholders(emailRequest.Body, emailRequest.Placeholders ?? new Dictionary<string, string>());

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(SmtpConfig.Username),
                    Subject = emailRequest.Subject,
                    Body = emailRequest.Body,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(emailRequest.To);

                if (emailRequest.Attachments != null && emailRequest.Attachments.Count > 0)
                {
                    foreach (var attachment in emailRequest.Attachments)
                    {
                        var memoryStream = new MemoryStream(attachment.Content);
                        var mailAttachment = new Attachment(memoryStream, attachment.FileName, attachment.MimeType);
                        mailMessage.Attachments.Add(mailAttachment);
                    }

                }

                await smtpClient.SendMailAsync(mailMessage);

                return new Response<bool>("Email sent successfully.");
            }
            catch (Exception ex)
            {
                return new Response<bool>($"An error occurred when sending the email: {ex.Message}");
            }
           
        }

        private string ReplacePlaceholders(string body, Dictionary<string, string> placeholders)
        {
            if (placeholders == null || placeholders.Count == 0)
                return body;

            foreach (var placeholder in placeholders)
            {
                body = body.Replace(placeholder.Key, placeholder.Value);
            }

            return body;
        }
    }
}
