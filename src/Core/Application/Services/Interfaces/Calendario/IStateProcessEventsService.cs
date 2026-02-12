using Application.DTOs.Request.Calendario;
using Application.DTOs.Response.Calendario;

namespace Application.Services.Interfaces.Calendario
{
    public interface IStateProcessEventsService : ICommonService<StateProcessEventsResponse, StateProcessEventsDTORequest, StateProcessEventsDTOUpdateRequest>
    {
    }
}
