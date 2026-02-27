using Application.DTOs.Request;
using Application.DTOs.Response;
using Application.Helpers.Wrappers;

namespace Application.Services.Interfaces
{
    public interface ILandingConfigService
    {
        Task<Response<LandingConfigDTOResponse>> GetLandingConfig();
        Task<Response<LandingConfigDTOResponse>> UpsertLandingConfig(LandingConfigDTORequest request);
    }
}
