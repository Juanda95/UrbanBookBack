using Application.DTOs.Request;
using Application.DTOs.Response;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UrbanBook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventoController(IEventsService EventoService) : Controller
    {
        private readonly IEventsService _EventoService = EventoService;

        //GET
        [Authorize]
        [HttpGet("GetEventoById/{Id}")]
        public async Task<IActionResult> GetClienteById(int Id)
        {
            var respuestaServicio = await _EventoService.GetById(Id);
            return StatusCode((int)respuestaServicio.HttpStatusCode, respuestaServicio);
        }

        //GET ALL
        [Authorize]
        [HttpGet]
        [Route("GetAllEventos")]
        public async Task<IActionResult> GetAllEventos()
        {
            var respuestaServicio = await _EventoService.GetAll();
            return StatusCode((int)respuestaServicio.HttpStatusCode, respuestaServicio);
        }

        // POST

        [Authorize]
        [HttpPost]
        [Route("InsertEvento")]
        public async Task<IActionResult> InsertEvento(EventoDTORequest EventoRequest)
        {
            var respuestaServicio = await _EventoService.CreateAsync(EventoRequest);
            return StatusCode((int)respuestaServicio.HttpStatusCode, respuestaServicio);
        }

        //PUT 
        [Authorize]
        [HttpPut]
        [Route("UpdateEvento")]
        public async Task<IActionResult> UpdateEvento(EventoDTOUpdateRequest EventoRequest)
        {
            var respuestaServicio = await _EventoService.UpdateAsync(EventoRequest);
            return StatusCode((int)respuestaServicio.HttpStatusCode, respuestaServicio);

        }

        //DELETE 
        [Authorize]
        [HttpDelete("DeleteEvento/{Id}")]
        public async Task<IActionResult> DeleteEvento(int Id)
        {
            var respuestaServicio = await _EventoService.DeleteAsync(Id);
            return StatusCode((int)respuestaServicio.HttpStatusCode, respuestaServicio);
        }

        // GET
        [Authorize]
        [HttpGet]
        [Route("GetAllActiveEvents")]
        public async Task<IActionResult> GetAllActiveEvents()
        {
            var respuestaServicio = await _EventoService.GetAllActiveEvents();
            return StatusCode((int)respuestaServicio.HttpStatusCode, respuestaServicio);
        }
    }
}
