using Application.DTOs.Request;
using Application.DTOs.Response;
using Application.Helpers.Wrappers;

namespace Application.Services.Interfaces
{
    public interface IAuthService
    {
        Task<Response<LoginDTOResponse>> Authenticate(LoginDTORequest DataLogin);
    }
}
