using Application.DTOs.Request;
using Application.DTOs.Response;
using Application.Helpers.Wrappers;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.Entities.Dcliente;
using Persistence.Repository.Interface;
using Persistence.UnitOfWork.Interface;
using System.Net;

namespace Application.Services
{
    public class ClienteService(IUnitOfWork unitOfWork, IMapper mapper) : ICommonService<ClienteDTOResponse, ClienteDTORequest, ClienteDTOUpdateRequest>
    {
        public async Task<Response<ClienteDTOResponse>> CreateAsync(ClienteDTORequest Request)
        {
            try
            {
                Cliente Nuevocliente = mapper.Map<Cliente>(Request);
                using (unitOfWork)
                {
                    IRepository<Cliente> userRepository = unitOfWork.GetRepository<Cliente>();
                    userRepository.Insert(Nuevocliente);
                    await unitOfWork.SaveChangesAsync();
                    var clienteResponse = mapper.Map<ClienteDTOResponse>(Nuevocliente);
                    return new Response<ClienteDTOResponse>(clienteResponse, "El cliente se creo con exito", HttpStatusCode.Created);
                }

            }
            catch (Exception ex)
            {
                throw new Exception($"Acurrido un error en el proceso de guardado del cliente {ex}");
            }
        }

        public async Task<Response<bool>> DeleteAsync(int Id)
        {
            try
            {
                using (unitOfWork)
                {
                    var clienteRepository = unitOfWork.GetRepository<Cliente>();
                    var cliente = await clienteRepository.FirstOrDefaultAsync(
                                    cliente => cliente.clienteId.Equals(Id));

                    if (cliente == null)
                    {
                        return new Response<bool>($"No se encontró ningún cliente con el ID {Id}", HttpStatusCode.NotFound);
                    }
                    clienteRepository.Delete(cliente);

                    await unitOfWork.SaveChangesAsync();

                    return new Response<bool>(true, "cliente Eliminado correctamente");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ocurrio un error al eliminar los datos del cliente {ex}");
            }
        }

        public async Task<Response<List<ClienteDTOResponse>>> GetAll()
        {
            try
            {
                using (unitOfWork)
                {
                    var clienteRepository = unitOfWork.GetRepository<Cliente>();
                    var clientes = await clienteRepository.GetAllAsync();

                    if (!clientes.Any())
                    {
                        return new Response<List<ClienteDTOResponse>>("No se encontraron clientes", HttpStatusCode.NotFound);
                    }
                    var clientesResponse = mapper.Map<List<ClienteDTOResponse>>(clientes);
                    return new Response<List<ClienteDTOResponse>>(clientesResponse, "clientes obtenidos con exito");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ha ocurrido un error en la consulta de los clientes {ex}");
            }
        }

        public async Task<Response<ClienteDTOResponse>> GetById(int Id)
        {
            try
            {
                using (unitOfWork)
                {

                    var clienteRepositorio = unitOfWork.GetRepository<Cliente>();
                    var cliente = await clienteRepositorio.FirstOrDefaultAsync(
                                            cliente => cliente.clienteId.Equals(Id));
                    if (cliente == null)
                    {
                        return new Response<ClienteDTOResponse>($"No se encontro informacion con el id {Id}", HttpStatusCode.NotFound);
                    }
                    var clienteResponse = mapper.Map<ClienteDTOResponse>(cliente);
                    return new Response<ClienteDTOResponse>(clienteResponse, "cliente obtenido con exito");

                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ha ocurrido un error al optener el dato del cliente {Id}, {ex}");
            }
        }

        public async Task<Response<bool>> UpdateAsync(ClienteDTOUpdateRequest UpdateRequest)
        {
            try
            {
                using (unitOfWork)
                {
                    var clienteRepository = unitOfWork.GetRepository<Cliente>();
                    var clienteExistente = await clienteRepository.FirstOrDefaultAsync(
                                            cliente => cliente.clienteId.Equals(UpdateRequest.ClienteId));

                    if (clienteExistente == null)
                    {
                        return new Response<bool>($"No se encontró ningún cliente con el ID {UpdateRequest.ClienteId}", HttpStatusCode.NotFound);
                    }
                    // Actualizar propiedades del usuario con los datos proporcionados
                    mapper.Map(UpdateRequest, clienteExistente);

                    await unitOfWork.SaveChangesAsync();

                    return new Response<bool>(true, "cliente actualizado correctamente");
                }

            }
            catch (Exception ex)
            {

                throw new Exception($"Ocurrió un error durante la actualización del cliente: {ex.Message}");
            }
        }
    }
}
