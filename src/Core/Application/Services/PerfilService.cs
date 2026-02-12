using Application.DTOs.Response;
using Application.Helpers.Wrappers;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.Entities.DUsuario  ;
using Persistence.UnitOfWork.Interface;

namespace Application.Services
{
    public class PerfilService(IUnitOfWork unitOfWork, IMapper mapper) : IPerfilService
    {

        public async Task<Response<IEnumerable<PerfilDTOResponse>>> GetAllPerfiles()
        {
            try
            {
                var PerfilRepository = unitOfWork.GetRepository<Perfil>();
                var perfilesExistentes = await PerfilRepository.GetAllAsync();
                var PerfilesDTO = perfilesExistentes.Select(perfil => mapper.Map<PerfilDTOResponse>(perfil)).ToList();
                return new Response<IEnumerable<PerfilDTOResponse>>(PerfilesDTO, string.Empty);
            }
            catch (Exception ex)
            {
                throw new Exception($"Acurrido un error en la consulta de perfil {ex}");
            }
        }

    }
}
