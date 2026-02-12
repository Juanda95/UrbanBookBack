using Application.Helpers.Wrappers;

namespace Application.Services.Interfaces.Firebase
{
    public interface IFileStorageService
    {
        Task<Response<string>> UploadFileAsync(Stream fileStream, string fileName);
    }
}
