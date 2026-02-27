using Application.DTOs.Request;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UrbanBook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LandingConfigController(ILandingConfigService landingConfigService) : Controller
    {
        private readonly ILandingConfigService _landingConfigService = landingConfigService;

        [AllowAnonymous]
        [HttpGet]
        [Route("GetPublicLandingConfig")]
        public async Task<IActionResult> GetPublicLandingConfig()
        {
            var response = await _landingConfigService.GetLandingConfig();
            return StatusCode((int)response.HttpStatusCode, response);
        }

        [Authorize]
        [HttpGet]
        [Route("GetLandingConfig")]
        public async Task<IActionResult> GetLandingConfig()
        {
            var response = await _landingConfigService.GetLandingConfig();
            return StatusCode((int)response.HttpStatusCode, response);
        }

        [Authorize]
        [HttpPost]
        [Route("UpsertLandingConfig")]
        public async Task<IActionResult> UpsertLandingConfig(LandingConfigDTORequest request)
        {
            var response = await _landingConfigService.UpsertLandingConfig(request);
            return StatusCode((int)response.HttpStatusCode, response);
        }
    }
}
