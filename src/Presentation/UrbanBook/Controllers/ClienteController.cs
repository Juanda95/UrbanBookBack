using Application.DTOs.Request;
using Application.DTOs.Response;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UrbanBook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class clienteController(ICommonService<ClienteDTOResponse, ClienteDTORequest, ClienteDTOUpdateRequest> clienteService) : Controller
    {
        private readonly ICommonService<ClienteDTOResponse, ClienteDTORequest, ClienteDTOUpdateRequest> _clienteService = clienteService;


        //GET
        [Authorize]
        [HttpGet("GetclienteById/{Id}")]
        public async Task<IActionResult> GetclienteById(int Id)
        {
            var respuestaServicio = await _clienteService.GetById(Id);
            return StatusCode((int)respuestaServicio.HttpStatusCode, respuestaServicio);
        }

        //GET ALL
        [Authorize]
        [HttpGet]
        [Route("GetAllclientes")]
        public async Task<IActionResult> GetAllclientes()
        {
            var respuestaServicio = await _clienteService.GetAll();
            return StatusCode((int)respuestaServicio.HttpStatusCode, respuestaServicio);
        }

        // POST

        [Authorize]
        [HttpPost]
        [Route("Insertcliente")]
        public async Task<IActionResult> Insertclientes(ClienteDTORequest Request)
        {
            var respuestaServicio = await _clienteService.CreateAsync(Request);
            return StatusCode((int)respuestaServicio.HttpStatusCode, respuestaServicio);
        }

        //PUT 
        [Authorize]
        [HttpPut]
        [Route("Updatecliente")]
        public async Task<IActionResult> Updatecliente(ClienteDTOUpdateRequest UpdateRequest)
        {
            var respuestaServicio = await _clienteService.UpdateAsync(UpdateRequest);
            return StatusCode((int)respuestaServicio.HttpStatusCode, respuestaServicio);

        }

        //DELETE 
        [Authorize]
        [HttpDelete("Deletecliente/{Id}")]
        public async Task<IActionResult> Deleteclientes(int Id)
        {
            var respuestaServicio = await _clienteService.DeleteAsync(Id);
            return StatusCode((int)respuestaServicio.HttpStatusCode, respuestaServicio);
        }
    }
}
