using Application.DTOs.Request;
using Application.DTOs.Response;
using Application.Helpers.Wrappers;

namespace Application.Services.Interfaces
{
    public interface IHorarioAtencionService
    {
        Task<Response<List<HorarioAtencionDTOResponse>>> GetByUsuarioId(int usuarioId);
        Task<Response<List<HorarioAtencionDTOResponse>>> SaveHorarios(HorarioAtencionBulkDTORequest request);
    }
}
