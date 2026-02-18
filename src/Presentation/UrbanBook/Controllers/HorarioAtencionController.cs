using Application.DTOs.Request;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UrbanBook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HorarioAtencionController(IHorarioAtencionService horarioAtencionService) : Controller
    {
        private readonly IHorarioAtencionService _horarioAtencionService = horarioAtencionService;

        [Authorize]
        [HttpGet("GetByUsuarioId/{usuarioId}")]
        public async Task<IActionResult> GetByUsuarioId(int usuarioId)
        {
            var response = await _horarioAtencionService.GetByUsuarioId(usuarioId);
            return StatusCode((int)response.HttpStatusCode, response);
        }

        [Authorize]
        [HttpPost]
        [Route("SaveHorarios")]
        public async Task<IActionResult> SaveHorarios(HorarioAtencionBulkDTORequest request)
        {
            var response = await _horarioAtencionService.SaveHorarios(request);
            return StatusCode((int)response.HttpStatusCode, response);
        }
    }
}
