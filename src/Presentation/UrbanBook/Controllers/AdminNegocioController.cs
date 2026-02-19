using Application.DTOs.Request;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UrbanBook.Controllers
{
    [ApiController]
    [Route("api/admin/negocio")]
    [Authorize(Roles = "SuperAdmin")]
    public class AdminNegocioController(INegocioService negocioService) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await negocioService.GetAll();
            return StatusCode((int)result.HttpStatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await negocioService.GetById(id);
            return StatusCode((int)result.HttpStatusCode, result);
        }

        [HttpGet("slug/{slug}")]
        public async Task<IActionResult> GetBySlug(string slug)
        {
            var result = await negocioService.GetBySlug(slug);
            return StatusCode((int)result.HttpStatusCode, result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] NegocioDTORequest request)
        {
            var result = await negocioService.CreateAsync(request);
            return StatusCode((int)result.HttpStatusCode, result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] NegocioDTOUpdateRequest request)
        {
            var result = await negocioService.UpdateAsync(request);
            return StatusCode((int)result.HttpStatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Deactivate(int id)
        {
            var result = await negocioService.DeactivateAsync(id);
            return StatusCode((int)result.HttpStatusCode, result);
        }
    }
}
