using Application.DTOs.Request;
using Application.DTOs.Response;

namespace Application.Services.Interfaces
{
    public interface IParameterService : ICommonService<ParameterDTOResponse, ParametersDTORequest, ParameterDTOUpdateRequest>
    {
    }
}
