using Application.DTOs.Request;
using Application.DTOs.Response;
using Application.Helpers.Wrappers;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.Entities.DServicio;
using Persistence.UnitOfWork.Interface;
using System.Net;

namespace Application.Services
{
    public class ServicioService(IUnitOfWork unitOfWork, IMapper mapper) : IServicioService
    {
        public async Task<Response<ServicioDTOResponse>> CreateAsync(ServicioDTORequest Request)
        {
            try
            {
                Servicio nuevoServicio = mapper.Map<Servicio>(Request);
                using (unitOfWork)
                {
                    var servicioRepository = unitOfWork.GetRepository<Servicio>();
                    servicioRepository.Insert(nuevoServicio);
                    await unitOfWork.SaveChangesAsync();
                    var servicioResponse = mapper.Map<ServicioDTOResponse>(nuevoServicio);
                    return new Response<ServicioDTOResponse>(servicioResponse, "El servicio se creó con éxito", HttpStatusCode.Created);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ha ocurrido un error en el proceso de guardado del servicio {ex}");
            }
        }

        public async Task<Response<bool>> DeleteAsync(int Id)
        {
            try
            {
                using (unitOfWork)
                {
                    var servicioRepository = unitOfWork.GetRepository<Servicio>();
                    var servicio = await servicioRepository.FirstOrDefaultAsync(
                                    s => s.ServicioId.Equals(Id));

                    if (servicio == null)
                    {
                        return new Response<bool>($"No se encontró ningún servicio con el ID {Id}", HttpStatusCode.NotFound);
                    }
                    servicioRepository.Delete(servicio);
                    await unitOfWork.SaveChangesAsync();
                    return new Response<bool>(true, "Servicio eliminado correctamente");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ocurrió un error al eliminar el servicio {ex}");
            }
        }

        public async Task<Response<List<ServicioDTOResponse>>> GetAll()
        {
            try
            {
                using (unitOfWork)
                {
                    var servicioRepository = unitOfWork.GetRepository<Servicio>();
                    var servicios = await servicioRepository.GetAllAsync();

                    if (!servicios.Any())
                    {
                        return new Response<List<ServicioDTOResponse>>("No se encontraron servicios", HttpStatusCode.NotFound);
                    }
                    var serviciosResponse = servicios.Select(s => mapper.Map<ServicioDTOResponse>(s)).ToList();
                    return new Response<List<ServicioDTOResponse>>(serviciosResponse, "Servicios obtenidos con éxito");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ha ocurrido un error en la consulta de los servicios {ex}");
            }
        }

        public async Task<Response<ServicioDTOResponse>> GetById(int Id)
        {
            try
            {
                using (unitOfWork)
                {
                    var servicioRepository = unitOfWork.GetRepository<Servicio>();
                    var servicio = await servicioRepository.FirstOrDefaultAsync(
                                            s => s.ServicioId.Equals(Id));
                    if (servicio == null)
                    {
                        return new Response<ServicioDTOResponse>($"No se encontró información con el id {Id}", HttpStatusCode.NotFound);
                    }
                    var servicioResponse = mapper.Map<ServicioDTOResponse>(servicio);
                    return new Response<ServicioDTOResponse>(servicioResponse, "Servicio obtenido con éxito");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ha ocurrido un error al obtener el servicio {Id}, {ex}");
            }
        }

        public async Task<Response<bool>> UpdateAsync(ServicioDTORequest UpdateRequest)
        {
            try
            {
                using (unitOfWork)
                {
                    var servicioRepository = unitOfWork.GetRepository<Servicio>();
                    var servicios = await servicioRepository.FindAllAsync(s => s.Nombre == UpdateRequest.Nombre);
                    var servicioExistente = servicios.FirstOrDefault();

                    if (servicioExistente == null)
                    {
                        return new Response<bool>("No se encontró el servicio a actualizar", HttpStatusCode.NotFound);
                    }

                    mapper.Map(UpdateRequest, servicioExistente);
                    await unitOfWork.SaveChangesAsync();
                    return new Response<bool>(true, "Servicio actualizado correctamente");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ocurrió un error durante la actualización del servicio: {ex.Message}");
            }
        }

        public async Task<Response<List<ServicioDTOResponse>>> GetActiveServices()
        {
            try
            {
                using (unitOfWork)
                {
                    var servicioRepository = unitOfWork.GetRepository<Servicio>();
                    var servicios = await servicioRepository.FindAllAsync(s => s.Activo == true);

                    if (!servicios.Any())
                    {
                        return new Response<List<ServicioDTOResponse>>("No se encontraron servicios activos", HttpStatusCode.NotFound);
                    }
                    var serviciosResponse = servicios.Select(s => mapper.Map<ServicioDTOResponse>(s)).ToList();
                    return new Response<List<ServicioDTOResponse>>(serviciosResponse, "Servicios obtenidos con éxito");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ha ocurrido un error al consultar los servicios activos {ex}");
            }
        }
    }
}
