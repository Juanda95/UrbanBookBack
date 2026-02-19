using Application.DTOs.Response;
using Application.Helpers.Wrappers;
using Domain.Interfaces;
using AutoMapper;
using Domain.Entities.DNegocio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;

namespace UrbanBook.Controllers
{
    /// <summary>
    /// Endpoint publico que retorna informacion basica del negocio (tenant).
    /// El frontend lo usa para personalizar la landing page y booking sin login.
    /// </summary>
    [ApiController]
    [Route("api/tenant")]
    public class TenantInfoController(ITenantService tenantService, UrbanBookDbContext dbContext, IMapper mapper) : Controller
    {
        [HttpGet("info")]
        public async Task<IActionResult> GetTenantInfo()
        {
            var tenantId = tenantService.GetCurrentTenantId();

            if (tenantId == 0)
            {
                return BadRequest(new Response<TenantInfoDTOResponse>("No se especifico un negocio. Acceda desde un subdominio valido."));
            }

            var negocio = await dbContext.Negocios
                .AsNoTracking()
                .FirstOrDefaultAsync(n => n.NegocioId == tenantId && n.Activo);

            if (negocio == null)
            {
                return NotFound(new Response<TenantInfoDTOResponse>("Negocio no encontrado"));
            }

            var dto = mapper.Map<TenantInfoDTOResponse>(negocio);
            return Ok(new Response<TenantInfoDTOResponse>(dto, "Informacion del negocio obtenida con exito"));
        }
    }
}
