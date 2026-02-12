using Application.DTOs.Request;
using Application.DTOs.Response;
using Application.Helpers.Wrappers;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.Entities.DUsuario;
using Persistence.Repository.Interface;
using Persistence.UnitOfWork.Interface;
using System.Net;

namespace Application.Services
{
    public class UsuarioService(IUnitOfWork unitOfWork, IMapper mapper) : ICommonService<UsuarioDTOResponse, UsuarioDTORequest, UsuarioDTOUpdateRequest>
    {

        public async Task<Response<List<UsuarioDTOResponse>>> GetAll()
        {
            try
            {
                using (unitOfWork)
                {

                    var usuarioRepository = unitOfWork.GetRepository<Usuario>();
                    var Usuarios = await usuarioRepository.GetAllAsync(
                                    Usuario => Usuario.Perfiles);

                    if (!Usuarios.Any())
                    {
                        return new Response<List<UsuarioDTOResponse>>("No se encontraron usuarios", HttpStatusCode.NotFound);
                    }
                    var UsuariosResponse = Usuarios.Select(usuario => mapper.Map<UsuarioDTOResponse>(usuario)).ToList();
                    return new Response<List<UsuarioDTOResponse>>(UsuariosResponse, "Usuarios obtenidos con exito");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ha ocurrido un error en la consulta de los usuarios {ex}");
            }
        }

        public async Task<Response<UsuarioDTOResponse>> GetById(int Id)
        {
            try
            {
                using (unitOfWork)
                {

                    var UsuarioRepositorio = unitOfWork.GetRepository<Usuario>();
                    var Usuario = await UsuarioRepositorio.FirstOrDefaultAsync(
                                            usuario => usuario.UsuarioId.Equals(Id),
                                            Usuario => Usuario.Perfiles
                                            );
                    if (Usuario == null)
                    {
                        return new Response<UsuarioDTOResponse>($"No se encontro informacion con el id {Id}", HttpStatusCode.NotFound);
                    }
                    var UsuarioResponse = mapper.Map<UsuarioDTOResponse>(Usuario);
                    return new Response<UsuarioDTOResponse>(UsuarioResponse, "Usuario obtenido con exito");

                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ha ocurrido un error al optener el dato {Id}, {ex}");
            }
        }

        public async Task<Response<UsuarioDTOResponse>> CreateAsync(UsuarioDTORequest UsuarioRequest)
        {
            try
            {
                Usuario NuevoUsuario = mapper.Map<Usuario>(UsuarioRequest);
                var perfilIds = UsuarioRequest.Perfiles.Select(p => p.PerfilId).ToList();
                using (unitOfWork)
                {
                    var PerfilRepository = unitOfWork.GetRepository<Perfil>();
                    var perfilesExistentes = await PerfilRepository.GetAllAsync();
                    var perfilesValidos = perfilesExistentes?.Where(p => perfilIds.Contains(p.PerfilId)).ToList();
                    if (perfilesValidos == null || perfilesValidos.Count == 0)
                    {
                        return new Response<UsuarioDTOResponse>("No se encontraron los perfiles que esta asociando al usuario", HttpStatusCode.NotFound);
                    }

                    NuevoUsuario.Perfiles = perfilesValidos;
                    IRepository<Usuario> userRepository = unitOfWork.GetRepository<Usuario>();
                    userRepository.Insert(NuevoUsuario);
                    await unitOfWork.SaveChangesAsync();
                    var UsuarioResponse = mapper.Map<UsuarioDTOResponse>(NuevoUsuario);
                    return new Response<UsuarioDTOResponse>(UsuarioResponse, "El usuario se creo con exito", HttpStatusCode.Created);

                } 

            }
            catch (Exception ex)
            {
                throw new Exception($"Acurrido un error en el proceso de guardado del usuario {ex}");
            }
        }

        public async Task<Response<bool>> UpdateAsync(UsuarioDTOUpdateRequest UsuarioUpdateRequest)
        {
            try
            {
                using (unitOfWork)
                {
                    var UsuarioRepository = unitOfWork.GetRepository<Usuario>();
                    var usuarioExistente = await UsuarioRepository.FirstOrDefaultAsync(
                                            usuario => usuario.UsuarioId.Equals(UsuarioUpdateRequest.UsuarioId),
                                            Usuario => Usuario.Perfiles
                                            );

                    if (usuarioExistente == null)
                    {
                        return new Response<bool>($"No se encontró ningún usuario con el ID {UsuarioUpdateRequest.UsuarioId}", HttpStatusCode.NotFound);
                    }
                    
                    // Actualizar propiedades del usuario con los datos proporcionados
                    mapper.Map(UsuarioUpdateRequest, usuarioExistente);

                    var perfilIds = UsuarioUpdateRequest.Perfiles.Select(p => p.PerfilId).ToList();
                    var PerfilRepository = unitOfWork.GetRepository<Perfil>();
                    var perfilesExistentes = await PerfilRepository.GetAllAsync();
                    var perfilesValidos = perfilesExistentes?.Where(p => perfilIds.Contains(p.PerfilId)).ToList();
                    if (perfilesValidos == null || perfilesValidos.Count == 0)
                    {
                        return new Response<bool>("No se encontraron los perfiles que se están asociando al usuario", HttpStatusCode.NotFound);

                    }

                    usuarioExistente.Perfiles = perfilesValidos;

                    await unitOfWork.SaveChangesAsync();

                    return new Response<bool>(true, "Usuario actualizado correctamente");
                }

            }
            catch (Exception ex)
            {

                throw new Exception($"Ocurrió un error durante la actualización del usuario: {ex.Message}");
            }
        }

        public async Task<Response<bool>> DeleteAsync(int Id)
        {
            try
            {
                using (unitOfWork)
                {
                    var UsuarioRepository = unitOfWork.GetRepository<Usuario>();
                    var Usuario = await UsuarioRepository.FirstOrDefaultAsync(
                                    usuario => usuario.UsuarioId.Equals(Id));

                    if (Usuario == null)
                    {
                        return new Response<bool>($"No se encontró ningún usuario con el ID {Id}", HttpStatusCode.NotFound);
                    }
                    UsuarioRepository.Delete(Usuario);

                    await unitOfWork.SaveChangesAsync();

                    return new Response<bool>(true, "Usuario Eliminado correctamente");
                }

            }
            catch (Exception ex)
            {

                throw new Exception($"Ocurrio un error al eliminar los datos {ex}");
            }
        }
    }
}
