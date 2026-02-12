using Application.DTOs.Request;
using Application.DTOs.Response;
using Application.Helpers.Wrappers;

namespace Application.Services.Interfaces
{
    public interface IEventsService : ICommonService<EventoDTOResponse, EventoDTORequest, EventoDTOUpdateRequest>
    {
        public Task<Response<List<EventoDTOResponse>>> GetAllActiveEvents();
    }
}
 