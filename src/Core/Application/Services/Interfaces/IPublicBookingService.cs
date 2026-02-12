using Application.DTOs.Request;
using Application.DTOs.Response;
using Application.Helpers.Wrappers;

namespace Application.Services.Interfaces
{
    public interface IPublicBookingService
    {
        Task<Response<List<ProfessionalPublicDTOResponse>>> GetPublicProfessionals();
        Task<Response<List<TimeSlotDTOResponse>>> GetAvailableSlots(int usuarioId, DateTime fecha, int duracionMinutos);
        Task<Response<ClienteDTOResponse>> PublicLogin(PublicLoginDTORequest request);
        Task<Response<ClienteDTOResponse>> PublicRegister(ClienteDTORequest request);
        Task<Response<EventoDTOResponse>> CreatePublicBooking(PublicBookingDTORequest request);
    }
}
