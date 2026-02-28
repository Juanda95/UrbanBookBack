using Application.DTOs.Response;
using Application.Helpers.Wrappers;

namespace Application.Services.Interfaces
{
    public interface IDashboardService
    {
        Task<Response<DashboardSummaryDTOResponse>> GetDashboardSummary(int year, int month);
    }
}
