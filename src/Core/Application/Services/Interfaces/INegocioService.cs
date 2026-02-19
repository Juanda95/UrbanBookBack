using Application.DTOs.Request;
using Application.DTOs.Response;
using Application.Helpers.Wrappers;

namespace Application.Services.Interfaces
{
    public interface INegocioService
    {
        Task<Response<List<NegocioDTOResponse>>> GetAll();
        Task<Response<NegocioDTOResponse>> GetById(int id);
        Task<Response<NegocioDTOResponse>> GetBySlug(string slug);
        Task<Response<NegocioDTOResponse>> CreateAsync(NegocioDTORequest request);
        Task<Response<bool>> UpdateAsync(NegocioDTOUpdateRequest request);
        Task<Response<bool>> DeactivateAsync(int id);
    }
}
