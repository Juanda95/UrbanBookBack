using Application.DTOs.Request;
using Application.DTOs.Response;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UrbanBook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController(ICommonService<UsuarioDTOResponse, UsuarioDTORequest, UsuarioDTOUpdateRequest> UsuarioService) : Controller
    {
        private readonly ICommonService<UsuarioDTOResponse, UsuarioDTORequest, UsuarioDTOUpdateRequest> _UsuarioService = UsuarioService;

        //GET
        [Authorize]
        [HttpGet("GetUsuarioById/{Id}")]
        public async Task<IActionResult> GetClienteById(int Id)
        {
            var respuestaServicio = await _UsuarioService.GetById(Id);
            return StatusCode((int)respuestaServicio.HttpStatusCode, respuestaServicio); 
        }

        //GET ALL
        //[Authorize]
        [HttpGet]
        [Route("GetAllUsuarios")]
        public async Task<IActionResult> GetAllUsuarios()
        {
            var respuestaServicio = await _UsuarioService.GetAll();
            return StatusCode((int)respuestaServicio.HttpStatusCode, respuestaServicio);
        }

        // POST

        [HttpPost]
        [Route("InsertUsuario")]
        public async Task<IActionResult> InsertUsuario(UsuarioDTORequest UsuarioRequest)
        {          
            var respuestaServicio = await _UsuarioService.CreateAsync(UsuarioRequest);
            return StatusCode((int)respuestaServicio.HttpStatusCode, respuestaServicio);
        }

        //PUT 
        [Authorize]
        [HttpPut]
        [Route("UpdateUsuario")]
        public async Task<IActionResult> UpdateUsuario(UsuarioDTOUpdateRequest UsuarioRequest)
        {
            var respuestaServicio = await _UsuarioService.UpdateAsync(UsuarioRequest);
            return StatusCode((int)respuestaServicio.HttpStatusCode, respuestaServicio);

        }

        //DELETE 
        [Authorize]
        [HttpDelete("DeleteUsuario/{Id}")]
        public async Task<IActionResult> DeleteUsuario(int Id)
        {
            var respuestaServicio = await _UsuarioService.DeleteAsync(Id);
            return StatusCode((int)respuestaServicio.HttpStatusCode, respuestaServicio);
        }

    } 
}
