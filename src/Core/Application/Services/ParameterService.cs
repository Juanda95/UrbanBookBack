using Application.DTOs.Request;
using Application.DTOs.Response;
using Application.Helpers.Wrappers;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.Entities.Parametros;
using Persistence.UnitOfWork.Interface;
using System.Net;

namespace Application.Services
{
    public class ParameterService(IUnitOfWork unitOfWork, IMapper mapper) : IParameterService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<Response<ParameterDTOResponse>> CreateAsync(ParametersDTORequest request)
        {
            try
            {
                using (_unitOfWork)
                {
                    var parameterRepository = _unitOfWork.GetRepository<Parameter>();
                    Parameter newParameter = _mapper.Map<Parameter>(request);
                    parameterRepository.Insert(newParameter);
                    await _unitOfWork.SaveChangesAsync();
                    var parameterResponse = _mapper.Map<ParameterDTOResponse>(newParameter);
                    return new Response<ParameterDTOResponse>(parameterResponse, "El parámetro se creó con éxito", HttpStatusCode.Created);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ocurrió un error en el proceso de creación del parámetro: {ex.Message}");
            }
        }

        public async Task<Response<bool>> DeleteAsync(int Id)
        {
            try
            {
                using (_unitOfWork)
                {
                    var parameterRepository = _unitOfWork.GetRepository<Parameter>();
                    var parameter = await parameterRepository.FirstOrDefaultAsync(p => p.IdParameter.Equals(Id));

                    if (parameter == null)
                    {
                        return new Response<bool>($"No se encontró ningún parámetro con el ID {Id}", HttpStatusCode.NotFound);
                    }

                    parameterRepository.Delete(parameter);
                    await _unitOfWork.SaveChangesAsync();
                    return new Response<bool>(true, "Parámetro eliminado correctamente");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ocurrió un error al eliminar el parámetro: {ex.Message}");
            }
        }

        public async Task<Response<List<ParameterDTOResponse>>> GetAll()
        {
            try
            {
                using (_unitOfWork)
                {
                    var parameterRepository = _unitOfWork.GetRepository<Parameter>();
                    var parameters = await parameterRepository.GetAllAsync(
                        Parameter => Parameter.Values);

                    if (!parameters.Any())
                    {
                        return new Response<List<ParameterDTOResponse>>("No se encontraron parámetros", HttpStatusCode.NotFound);
                    }

                    var parametersResponse = parameters.Select(p => _mapper.Map<ParameterDTOResponse>(p)).ToList();
                    return new Response<List<ParameterDTOResponse>>(parametersResponse, "Parámetros obtenidos con éxito");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ocurrió un error en la consulta de parámetros: {ex.Message}");
            }
        }

        public async Task<Response<ParameterDTOResponse>> GetById(int Id)
        {
            try
            {
                using (_unitOfWork)
                {
                    var parameterRepository = _unitOfWork.GetRepository<Parameter>();
                    var parameter = await parameterRepository.FirstOrDefaultAsync(p => p.IdParameter.Equals(Id));

                    if (parameter == null)
                    {
                        return new Response<ParameterDTOResponse>($"No se encontró información para el ID {Id}", HttpStatusCode.NotFound);
                    }

                    var parameterResponse = _mapper.Map<ParameterDTOResponse>(parameter);
                    return new Response<ParameterDTOResponse>(parameterResponse, "Parámetro obtenido con éxito");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ocurrió un error al obtener el parámetro: {ex.Message}");
            }
        }
                                                      
        public async Task<Response<bool>> UpdateAsync(ParameterDTOUpdateRequest updateRequest)
        {
            try
            {
                using (_unitOfWork)
                {
                    var parameterRepository = _unitOfWork.GetRepository<Parameter>();
                    var existingParameter = await parameterRepository.FirstOrDefaultAsync(p => p.IdParameter.Equals(updateRequest.IdParameter),
                        parameter => parameter.Values);

                    if (existingParameter == null)
                    {
                        return new Response<bool>($"No se encontró ningún parámetro con el ID {updateRequest.IdParameter}", HttpStatusCode.NotFound);
                    }

                    if (updateRequest.Values == null || !updateRequest.Values.Any())
                    {
                        return new Response<bool>("Debe proporcionar al menos un valor para actualizar el parámetro", HttpStatusCode.BadRequest);
                    }
                    // Identifica los valores existentes para actualizar o eliminar
                    var existingValuesIds = existingParameter.Values.Select(v => v.IdValue).ToList();
                    var updatedValuesIds = updateRequest.Values.Where(v => v.IdValue != 0).Select(v => v.IdValue).ToList();

                    // Elimina los valores que no están en el updateRequest
                    existingParameter.Values.RemoveAll(v => !updatedValuesIds.Contains(v.IdValue));

                    foreach (var valueDto in updateRequest.Values)
                    {
                        if (valueDto.IdValue == 0)
                        {
                            // Agrega un nuevo valor
                            var newValue = _mapper.Map<Value>(valueDto);
                            existingParameter.Values.Add(newValue);
                        }
                        else
                        {
                            // Actualiza el valor existente
                            var existingValue = existingParameter.Values.FirstOrDefault(v => v.IdValue == valueDto.IdValue);
                            if (existingValue != null)
                            {
                                _mapper.Map(valueDto, existingValue);
                            }
                        }
                    }

                    _mapper.Map(updateRequest, existingParameter);
                    await _unitOfWork.SaveChangesAsync();
                    return new Response<bool>(true, "Parámetro actualizado correctamente");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ocurrió un error durante la actualización del parámetro: {ex.Message}");
            }
        }
    }
}
