using Application.DTOs.Request;
using Application.DTOs.Response;
using Application.Helpers.Wrappers;

namespace Application.Services.Interfaces
{
    public interface IServicioService : ICommonService<ServicioDTOResponse, ServicioDTORequest, ServicioDTORequest>
    {
        Task<Response<List<ServicioDTOResponse>>> GetActiveServices();
    }
}
