using Application.Services.Interfaces.Calendario;
using Domain.Entities.DCalendario;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UrbanBook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StateProcessEventController(IStateProcessEventsService stateProcessEventsService) : Controller
    {
        private readonly IStateProcessEventsService _stateProcessEventsService = stateProcessEventsService;

        //GET
        [Authorize]
        [HttpGet]
        [Route("GetAllStateProcessEvents")]
        public async Task<IActionResult> GetAllStateProcessEvents()
        {
            var respuestaServicio = await _stateProcessEventsService.GetAll();
            return StatusCode((int)respuestaServicio.HttpStatusCode, respuestaServicio);
        }
        
    }
}
