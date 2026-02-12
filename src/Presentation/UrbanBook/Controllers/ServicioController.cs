using Application.DTOs.Request;
using Application.DTOs.Response;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UrbanBook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicioController(IServicioService servicioService) : Controller
    {
        private readonly IServicioService _servicioService = servicioService;

        //GET - Público (sin autenticación)
        [AllowAnonymous]
        [HttpGet]
        [Route("GetPublicServices")]
        public async Task<IActionResult> GetPublicServices()
        {
            var respuestaServicio = await _servicioService.GetActiveServices();
            return StatusCode((int)respuestaServicio.HttpStatusCode, respuestaServicio);
        }

        //GET ALL
        [Authorize]
        [HttpGet]
        [Route("GetAllServicios")]
        public async Task<IActionResult> GetAllServicios()
        {
            var respuestaServicio = await _servicioService.GetAll();
            return StatusCode((int)respuestaServicio.HttpStatusCode, respuestaServicio);
        }

        //GET
        [Authorize]
        [HttpGet("GetServicioById/{Id}")]
        public async Task<IActionResult> GetServicioById(int Id)
        {
            var respuestaServicio = await _servicioService.GetById(Id);
            return StatusCode((int)respuestaServicio.HttpStatusCode, respuestaServicio);
        }

        // POST
        [Authorize]
        [HttpPost]
        [Route("InsertServicio")]
        public async Task<IActionResult> InsertServicio(ServicioDTORequest servicioRequest)
        {
            var respuestaServicio = await _servicioService.CreateAsync(servicioRequest);
            return StatusCode((int)respuestaServicio.HttpStatusCode, respuestaServicio);
        }

        //DELETE
        [Authorize]
        [HttpDelete("DeleteServicio/{Id}")]
        public async Task<IActionResult> DeleteServicio(int Id)
        {
            var respuestaServicio = await _servicioService.DeleteAsync(Id);
            return StatusCode((int)respuestaServicio.HttpStatusCode, respuestaServicio);
        }
    }
}
