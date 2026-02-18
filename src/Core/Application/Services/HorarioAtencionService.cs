using Application.DTOs.Request;
using Application.DTOs.Response;
using Application.Helpers.Wrappers;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.Entities.DUsuario;
using Persistence.UnitOfWork.Interface;
using System.Net;

namespace Application.Services
{
    public class HorarioAtencionService(IUnitOfWork unitOfWork, IMapper mapper) : IHorarioAtencionService
    {
        public async Task<Response<List<HorarioAtencionDTOResponse>>> GetByUsuarioId(int usuarioId)
        {
            try
            {
                using (unitOfWork)
                {
                    var usuarioRepository = unitOfWork.GetRepository<Usuario>();
                    var usuario = await usuarioRepository.FirstOrDefaultAsync(u => u.UsuarioId == usuarioId);
                    if (usuario == null)
                    {
                        return new Response<List<HorarioAtencionDTOResponse>>(
                            $"No se encontró el usuario con ID {usuarioId}", HttpStatusCode.NotFound);
                    }

                    var horarioRepository = unitOfWork.GetRepository<HorarioAtencion>();
                    var exclusionRepository = unitOfWork.GetRepository<ExclusionHorario>();

                    var horarios = await horarioRepository.FindAllAsync(h => h.UsuarioId == usuarioId);
                    var horariosList = horarios.ToList();

                    if (!horariosList.Any())
                    {
                        return new Response<List<HorarioAtencionDTOResponse>>(
                            new List<HorarioAtencionDTOResponse>(), "No hay horarios configurados para este usuario");
                    }

                    var horarioIds = horariosList.Select(h => h.HorarioAtencionId).ToList();
                    var exclusiones = await exclusionRepository.FindAllAsync(
                        e => horarioIds.Contains(e.HorarioAtencionId));
                    var exclusionesList = exclusiones.ToList();

                    // Ensamblar exclusiones en cada horario
                    foreach (var horario in horariosList)
                    {
                        horario.Exclusiones = exclusionesList
                            .Where(e => e.HorarioAtencionId == horario.HorarioAtencionId)
                            .ToList();
                    }

                    var horariosResponse = horariosList
                        .Select(h => mapper.Map<HorarioAtencionDTOResponse>(h))
                        .ToList();

                    return new Response<List<HorarioAtencionDTOResponse>>(
                        horariosResponse, "Horarios obtenidos con éxito");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ha ocurrido un error al consultar los horarios de atención: {ex}");
            }
        }

        public async Task<Response<List<HorarioAtencionDTOResponse>>> SaveHorarios(HorarioAtencionBulkDTORequest request)
        {
            try
            {
                // Validaciones
                if (request.Horarios == null || !request.Horarios.Any())
                {
                    return new Response<List<HorarioAtencionDTOResponse>>(
                        "Debe enviar al menos un horario", HttpStatusCode.BadRequest);
                }

                // Validar días duplicados
                var diasDuplicados = request.Horarios
                    .GroupBy(h => h.DiaSemana)
                    .Where(g => g.Count() > 1)
                    .Select(g => g.Key);

                if (diasDuplicados.Any())
                {
                    return new Response<List<HorarioAtencionDTOResponse>>(
                        "No puede haber días duplicados en la configuración", HttpStatusCode.BadRequest);
                }

                // Validar rangos de hora
                foreach (var horario in request.Horarios)
                {
                    if (horario.DiaSemana < 0 || horario.DiaSemana > 6)
                    {
                        return new Response<List<HorarioAtencionDTOResponse>>(
                            $"Día de semana inválido: {horario.DiaSemana}", HttpStatusCode.BadRequest);
                    }

                    if (horario.Activo && horario.HoraInicio >= horario.HoraFin)
                    {
                        return new Response<List<HorarioAtencionDTOResponse>>(
                            $"La hora de inicio debe ser anterior a la hora de fin para el día {horario.DiaSemana}",
                            HttpStatusCode.BadRequest);
                    }

                    foreach (var exclusion in horario.Exclusiones)
                    {
                        if (exclusion.HoraInicio >= exclusion.HoraFin)
                        {
                            return new Response<List<HorarioAtencionDTOResponse>>(
                                "La hora de inicio de la exclusión debe ser anterior a la hora de fin",
                                HttpStatusCode.BadRequest);
                        }

                        if (exclusion.HoraInicio < horario.HoraInicio || exclusion.HoraFin > horario.HoraFin)
                        {
                            return new Response<List<HorarioAtencionDTOResponse>>(
                                "Las exclusiones deben estar dentro del rango del horario laboral del día",
                                HttpStatusCode.BadRequest);
                        }
                    }
                }

                using (unitOfWork)
                {
                    var usuarioRepository = unitOfWork.GetRepository<Usuario>();
                    var usuario = await usuarioRepository.FirstOrDefaultAsync(u => u.UsuarioId == request.UsuarioId);
                    if (usuario == null)
                    {
                        return new Response<List<HorarioAtencionDTOResponse>>(
                            $"No se encontró el usuario con ID {request.UsuarioId}", HttpStatusCode.NotFound);
                    }

                    var horarioRepository = unitOfWork.GetRepository<HorarioAtencion>();
                    var exclusionRepository = unitOfWork.GetRepository<ExclusionHorario>();

                    // Eliminar exclusiones existentes primero
                    var horariosExistentes = await horarioRepository.FindAllAsync(h => h.UsuarioId == request.UsuarioId);
                    var horariosExistentesList = horariosExistentes.ToList();

                    if (horariosExistentesList.Any())
                    {
                        var horarioIds = horariosExistentesList.Select(h => h.HorarioAtencionId).ToList();
                        var exclusionesExistentes = await exclusionRepository.FindAllAsync(
                            e => horarioIds.Contains(e.HorarioAtencionId));

                        foreach (var exclusion in exclusionesExistentes)
                        {
                            exclusionRepository.Delete(exclusion);
                        }

                        foreach (var horario in horariosExistentesList)
                        {
                            horarioRepository.Delete(horario);
                        }

                        await unitOfWork.SaveChangesAsync();
                    }

                    // Insertar nuevos horarios
                    var nuevosHorarios = new List<HorarioAtencion>();
                    foreach (var horarioDto in request.Horarios)
                    {
                        var nuevoHorario = mapper.Map<HorarioAtencion>(horarioDto);
                        nuevoHorario.UsuarioId = request.UsuarioId;
                        horarioRepository.Insert(nuevoHorario);
                        nuevosHorarios.Add(nuevoHorario);
                    }

                    await unitOfWork.SaveChangesAsync();

                    // Insertar exclusiones para cada horario
                    for (int i = 0; i < request.Horarios.Count; i++)
                    {
                        foreach (var exclusionDto in request.Horarios[i].Exclusiones)
                        {
                            var nuevaExclusion = mapper.Map<ExclusionHorario>(exclusionDto);
                            nuevaExclusion.HorarioAtencionId = nuevosHorarios[i].HorarioAtencionId;
                            exclusionRepository.Insert(nuevaExclusion);
                        }
                    }

                    await unitOfWork.SaveChangesAsync();

                    // Recargar para respuesta
                    var horariosGuardados = await horarioRepository.FindAllAsync(h => h.UsuarioId == request.UsuarioId);
                    var horariosGuardadosList = horariosGuardados.ToList();
                    var nuevosHorarioIds = horariosGuardadosList.Select(h => h.HorarioAtencionId).ToList();
                    var exclusionesGuardadas = await exclusionRepository.FindAllAsync(
                        e => nuevosHorarioIds.Contains(e.HorarioAtencionId));
                    var exclusionesGuardadasList = exclusionesGuardadas.ToList();

                    foreach (var horario in horariosGuardadosList)
                    {
                        horario.Exclusiones = exclusionesGuardadasList
                            .Where(e => e.HorarioAtencionId == horario.HorarioAtencionId)
                            .ToList();
                    }

                    var horariosResponse = horariosGuardadosList
                        .Select(h => mapper.Map<HorarioAtencionDTOResponse>(h))
                        .ToList();

                    return new Response<List<HorarioAtencionDTOResponse>>(
                        horariosResponse, "Horarios guardados con éxito", HttpStatusCode.Created);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ha ocurrido un error al guardar los horarios de atención: {ex}");
            }
        }
    }
}
