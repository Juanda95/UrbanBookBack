using Application.DTOs.Response;
using Application.Helpers.Wrappers;

namespace Application.Services.Interfaces
{
    public interface IPerfilService
    {
        Task<Response<IEnumerable<PerfilDTOResponse>>> GetAllPerfiles();
    }
}
