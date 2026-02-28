using Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UrbanBook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController(IDashboardService dashboardService) : Controller
    {
        private readonly IDashboardService _dashboardService = dashboardService;

        [Authorize]
        [HttpGet("GetSummary")]
        public async Task<IActionResult> GetSummary(
            [FromQuery] int? year = null,
            [FromQuery] int? month = null)
        {
            var y = year ?? DateTime.UtcNow.Year;
            var m = month ?? DateTime.UtcNow.Month;
            var respuestaServicio = await _dashboardService.GetDashboardSummary(y, m);
            return StatusCode((int)respuestaServicio.HttpStatusCode, respuestaServicio);
        }
    }
}
