using Application.DTOs.Request;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UrbanBook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParameterController(IParameterService parameterService) : Controller
    {

        private readonly IParameterService _ParameterService = parameterService;

        //GET
        [HttpGet("GetParameterById/{Id}")]
        public async Task<IActionResult> GetParameterById(int Id)
        {
            var respuestaServicio = await _ParameterService.GetById(Id);
            return StatusCode((int)respuestaServicio.HttpStatusCode, respuestaServicio);
        }

        //GET ALL
        [HttpGet]
        [Route("GetAllParameters")]
        public async Task<IActionResult> GetAllParameters()
        {
            var respuestaServicio = await _ParameterService.GetAll();
            return StatusCode((int)respuestaServicio.HttpStatusCode, respuestaServicio);
        }

        // POST

        [Authorize]
        [HttpPost]
        [Route("InsertParameter")]
        public async Task<IActionResult> InsertParameters(ParametersDTORequest Request)
        {
            var respuestaServicio = await _ParameterService.CreateAsync(Request);
            return StatusCode((int)respuestaServicio.HttpStatusCode, respuestaServicio);
        }

        //PUT 
        [Authorize]
        [HttpPut]
        [Route("UpdateParameter")]
        public async Task<IActionResult> UpdateParameter(ParameterDTOUpdateRequest UpdateRequest)
        {
            var respuestaServicio = await _ParameterService.UpdateAsync(UpdateRequest);
            return StatusCode((int)respuestaServicio.HttpStatusCode, respuestaServicio);

        }

        //DELETE 
        [Authorize]
        [HttpDelete("DeleteParameter/{Id}")]
        public async Task<IActionResult> DeleteParameters(int Id)
        {
            var respuestaServicio = await _ParameterService.DeleteAsync(Id);
            return StatusCode((int)respuestaServicio.HttpStatusCode, respuestaServicio);
        }


    }
}
