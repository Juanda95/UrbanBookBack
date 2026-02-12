using Application.DTOs.Request;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace UrbanBook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService loginService) : Controller
    {
        private readonly IAuthService _loginService = loginService;

        // POST
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginDTORequest DataLogin)
        {
            var respuestaServicio = await _loginService.Authenticate(DataLogin);
            return StatusCode((int)respuestaServicio.HttpStatusCode, respuestaServicio);
        }
    }
}
