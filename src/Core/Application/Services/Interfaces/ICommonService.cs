using Application.Helpers.Wrappers;

namespace Application.Services.Interfaces
{
    public interface ICommonService<TResponse,TRequest,TUpdate>
    {
        Task<Response<List<TResponse>>> GetAll();
        Task<Response<TResponse>> GetById(int Id);
        Task<Response<TResponse>> CreateAsync(TRequest Request);
        Task<Response<bool>> DeleteAsync(int Id);
        Task<Response<bool>> UpdateAsync(TUpdate UpdateRequest);
    }
}
