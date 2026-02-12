using Application.DTOs.Request.Messaging;
using Application.Services.Interfaces.Messaging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UrbanBook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController(IEmailService emailService) : Controller
    {
        private readonly IEmailService _emailService = emailService;

        [Authorize]
        [HttpPost]
        [Route("SendEmail")]
        public async Task<IActionResult> SendEmail(SendEmailDTORequest request)
        {
            var response = await _emailService.SendEmailAsync(request);
            return StatusCode((int)response.HttpStatusCode, response);
        }
    }
}
