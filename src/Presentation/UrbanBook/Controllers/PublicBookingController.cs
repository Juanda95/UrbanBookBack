using Application.DTOs.Request;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UrbanBook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublicBookingController(IPublicBookingService bookingService) : Controller
    {
        private readonly IPublicBookingService _bookingService = bookingService;

        //GET - Profesionales públicos
        [AllowAnonymous]
        [HttpGet]
        [Route("GetProfessionals")]
        public async Task<IActionResult> GetProfessionals()
        {
            var respuestaServicio = await _bookingService.GetPublicProfessionals();
            return StatusCode((int)respuestaServicio.HttpStatusCode, respuestaServicio);
        }

        //GET - Horarios disponibles
        [AllowAnonymous]
        [HttpGet]
        [Route("GetAvailableSlots")]
        public async Task<IActionResult> GetAvailableSlots(
            [FromQuery] int usuarioId,
            [FromQuery] DateTime fecha,
            [FromQuery] int duracionMinutos)
        {
            var respuestaServicio = await _bookingService.GetAvailableSlots(usuarioId, fecha, duracionMinutos);
            return StatusCode((int)respuestaServicio.HttpStatusCode, respuestaServicio);
        }

        //POST - Login público de cliente
        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(PublicLoginDTORequest request)
        {
            var respuestaServicio = await _bookingService.PublicLogin(request);
            return StatusCode((int)respuestaServicio.HttpStatusCode, respuestaServicio);
        }

        //POST - Registro público de cliente
        [AllowAnonymous]
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(ClienteDTORequest request)
        {
            var respuestaServicio = await _bookingService.PublicRegister(request);
            return StatusCode((int)respuestaServicio.HttpStatusCode, respuestaServicio);
        }

        //POST - Crear reserva pública
        [AllowAnonymous]
        [HttpPost]
        [Route("CreateBooking")]
        public async Task<IActionResult> CreateBooking(PublicBookingDTORequest request)
        {
            var respuestaServicio = await _bookingService.CreatePublicBooking(request);
            return StatusCode((int)respuestaServicio.HttpStatusCode, respuestaServicio);
        }
    }
}
