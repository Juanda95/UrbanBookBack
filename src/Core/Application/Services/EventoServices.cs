using Application.DTOs.Request;
using Application.DTOs.Request.Messaging;
using Application.DTOs.Response;
using Application.Helpers.Wrappers;
using Application.Services.Interfaces;
using Application.Services.Interfaces.Messaging;
using AutoMapper;
using Domain.Entities.DCalendario;
using Domain.Entities.Dcliente;
using Domain.Entities.DUsuario;
using Microsoft.EntityFrameworkCore;
using Persistence.Repository.Interface;
using Persistence.UnitOfWork.Interface;
using System.Net;

namespace Application.Services
{
    public class EventoServices(IUnitOfWork unitOfWork, IMapper mapper, IEmailService emailService) : IEventsService
    {
        private readonly IEmailService _emailService = emailService;
        public async Task<Response<EventoDTOResponse>> CreateAsync(EventoDTORequest request)
        {
            try
            {
                // Validación 1: Validar que FechaInicio < FechaFin
                if (request.FechaInicio >= request.FechaFin)
                {
                    return new Response<EventoDTOResponse>("La fecha de fin debe ser posterior a la de inicio", HttpStatusCode.BadRequest);
                }

                // Validación 2: Validar que no sea en el pasado
                if (request.FechaInicio < DateTime.UtcNow)
                {
                    return new Response<EventoDTOResponse>("No puedes agendar citas en el pasado", HttpStatusCode.BadRequest);
                }

                // Validación 3: Validar duración mínima (30 minutos)
                var duracion = request.FechaFin - request.FechaInicio;
                if (duracion.TotalMinutes < 30)
                {
                    return new Response<EventoDTOResponse>("La cita debe tener una duración mínima de 30 minutos", HttpStatusCode.BadRequest);
                }

                // Validación 4: Validar que no sea más de 6 meses en el futuro
                var fechaMaxima = DateTime.UtcNow.AddMonths(6);
                if (request.FechaInicio > fechaMaxima)
                {
                    return new Response<EventoDTOResponse>("No puedes agendar citas con más de 6 meses de anticipación", HttpStatusCode.BadRequest);
                }

                var clienteRepositorio = unitOfWork.GetRepository<Cliente>();
                var cliente = await clienteRepositorio.FirstOrDefaultAsync(
                                        cliente => cliente.clienteId.Equals(request.ClienteId));
                if (cliente == null)
                {
                    return new Response<EventoDTOResponse>($"El cliente {request.ClienteId} no se encuentra registrado", HttpStatusCode.NotFound);
                }

                var usuarioRepositorio = unitOfWork.GetRepository<Usuario>();
                var usuario = await usuarioRepositorio.FirstOrDefaultAsync(u => u.UsuarioId == request.UsuarioId);

                if (usuario == null)
                {
                    return new Response<EventoDTOResponse>($"El usuario {request.UsuarioId} no se encuentra registrado", HttpStatusCode.NotFound);
                }

                // Validación: Asignar StateProcessEventId por defecto si no se proporciona
                if (request.StateProcessEventId == null || request.StateProcessEventId == 0)
                {
                    var stateProcessRepository = unitOfWork.GetRepository<StateProcessEvents>();
                    var estadosProceso = await stateProcessRepository.GetAllAsync();
                    
                    if (!estadosProceso.Any())
                    {
                        return new Response<EventoDTOResponse>(
                            "No hay estados de proceso disponibles. Por favor, contacta al administrador.", 
                            HttpStatusCode.BadRequest);
                    }
                    
                    // Asignar el primer estado disponible
                    request.StateProcessEventId = estadosProceso.First().StateProcessEventsId;
                }
                else
                {
                    // Validar que el StateProcessEventId exista
                    var stateProcessRepository = unitOfWork.GetRepository<StateProcessEvents>();
                    var estadoProceso = await stateProcessRepository.FirstOrDefaultAsync(
                        sp => sp.StateProcessEventsId == request.StateProcessEventId);
                    
                    if (estadoProceso == null)
                    {
                        return new Response<EventoDTOResponse>(
                            $"El estado de proceso {request.StateProcessEventId} no existe", 
                            HttpStatusCode.NotFound);
                    }
                }

                var eventoRepositorio = unitOfWork.GetRepository<Evento>();

                var eventoEnConflicto = await eventoRepositorio.FirstOrDefaultAsync(e =>
                e.UsuarioId == request.UsuarioId &&
                e.Estado == true &&
                ((request.FechaInicio < e.FechaFin && request.FechaFin > e.FechaInicio) ||
                (e.FechaInicio < request.FechaFin && e.FechaFin > request.FechaInicio)));

                if (eventoEnConflicto != null)
                {
                    return new Response<EventoDTOResponse>("Ya existe un evento en la franja horaria especificada para este usuario", HttpStatusCode.Conflict);
                }

                Evento NuevoEvento = mapper.Map<Evento>(request);
                IRepository<Evento> userRepository = unitOfWork.GetRepository<Evento>();
                userRepository.Insert(NuevoEvento);

                await unitOfWork.SaveChangesAsync();
             
                // ENVIAR EMAIL DE CONFIRMACION usando SmtpConfig del tenant
                try
                {
                    var smtpConfigRepo = unitOfWork.GetRepository<Domain.Entities.DMessaging.SmtpConfig>();
                    var smtpConfig = (await smtpConfigRepo.GetAllAsync()).FirstOrDefault();

                    if (smtpConfig != null)
                    {
                        string htmlBody = GenerarEmailConfirmacion(NuevoEvento, cliente);

                        await _emailService.SendEmailAsync(new SendEmailDTORequest
                        {
                            SmtpConfigId = smtpConfig.SmtpConfigId,
                            To = cliente.Correo,
                            Subject = $"Confirmación de Cita Agendada - {NuevoEvento.Titulo}",
                            Body = htmlBody
                        });
                    }
                }
                catch (Exception emailEx)
                {
                    // Log el error de email pero no fallar la cita
                    Console.WriteLine($"Error al enviar email de confirmación: {emailEx.Message}");
                }

                var EventoResponse = mapper.Map<EventoDTOResponse>(NuevoEvento);
                return new Response<EventoDTOResponse>(EventoResponse, "El evento se creó con éxito", HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error detallado en CreateAsync: {ex.Message} | {ex.StackTrace}");
                return new Response<EventoDTOResponse>(
                    "Error al agendar la cita. Por favor intenta más tarde.",
                    HttpStatusCode.InternalServerError);
            }
        }

        private string GenerarEmailConfirmacion(Evento evento, Cliente cliente)
        {
            string htmlBody = $@"<!DOCTYPE html>
            <html lang='es'>
            <head>
                <meta charset='UTF-8'>
                <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                <title>Confirmación de Cita Agendada</title>
            </head>
            <body style='font-family: Arial, sans-serif; color: #333; background-color: #f6f8fc; padding: 20px;'>
                <table style='max-width: 600px; margin: 0 auto; border: 1px solid #ddd; border-radius: 10px; background-color: #fff;'>
                    <tr>
                        <td style='padding: 20px; vertical-align: top; width: 100px;'>
                            <img src='https://firebasestorage.googleapis.com/v0/b/UrbanBook-189d7.appspot.com/o/logopdfPrueba.jpg?alt=media&token=1a54ffa0-4791-4a9c-a22f-36a90375188d' alt='Logo de la Empresa' style='width: 86px;'>
                        </td>
                        <td style='padding: 20px; text-align: center;'>
                            <h2 style='color: #22A5F0; margin: 0;'>Confirmación de Cita Agendada</h2>
                        </td>
                    </tr>
                    <tr>
                        <td colspan='2' style='padding: 0 20px 20px 20px;'>
                            <p>Estimado/a {cliente.Nombres} {cliente.PrimerApellido},</p>
                            <p>Tu cita ha sido agendada exitosamente. Aquí están los detalles:</p>
                            <div style='background-color: #f0f0f0; padding: 15px; border-radius: 5px; margin: 20px 0;'>
                                <p><strong>Procedimiento:</strong> {evento.Titulo}</p>
                                <p><strong>Fecha:</strong> {evento.FechaInicio:dd/MM/yyyy}</p>
                                <p><strong>Hora de inicio:</strong> {evento.FechaInicio:HH:mm}</p>
                                <p><strong>Hora de fin:</strong> {evento.FechaFin:HH:mm}</p>
                                {(string.IsNullOrWhiteSpace(evento.Descripcion) ? "" : $"<p><strong>Descripción:</strong> {evento.Descripcion}</p>")}
                            </div>
                            <p>Si tienes alguna pregunta o necesitas modificar tu cita, no dude en contactarnos. Estamos aquí para ayudarte.</p>
                            <p>Gracias por tu confianza.</p>
                            <p>Saludos cordiales,<br>UrbanBook</p>
                            <hr style='border: none; border-top: 1px solid #ddd; margin-top: 20px;'>
                            <p style='font-size: 12px; color: #777;'>Este correo electrónico se generó automáticamente, por favor no responda a este mensaje. Si necesita ponerse en contacto con nosotros, utilice los canales de atención habituales.</p>
                        </td>
                    </tr>
                </table>
            </body>
            </html>";

            return htmlBody;
        }

        public async Task<Response<bool>> DeleteAsync(int Id)
        {
            try
            {
                using (unitOfWork)
                {
                    var EventoRepository = unitOfWork.GetRepository<Evento>();
                    var Evento = await EventoRepository.FirstOrDefaultAsync(
                                    Evento => Evento.EventoId.Equals(Id));

                    if (Evento == null)
                    {
                        return new Response<bool>($"No se encontró ningún evento con el ID {Id}", HttpStatusCode.NotFound);
                    }
                    EventoRepository.Delete(Evento);

                    await unitOfWork.SaveChangesAsync();

                    return new Response<bool>(true, "Evento eliminado correctamente");
                }

            }
            catch (Exception ex)
            {

                throw new Exception($"Ocurrio un error al eliminar los datos del evento {ex}");
            }
        }

        public async Task<Response<List<EventoDTOResponse>>> GetAll()
        {
            try
            {
                using (unitOfWork)
                {
                    var EventoRepository = unitOfWork.GetRepository<Evento>();
                    
                    // Cargar eventos con relación de cliente de forma eficiente
                    var Eventos = await EventoRepository.GetAllAsyncThenInclude(
                        evento => evento.Include(e => e.Cliente)
                    );

                    // Mapear con la relación de cliente incluida
                    var EventosResponse = Eventos.Select(Evento => mapper.Map<EventoDTOResponse>(Evento)).ToList();
                    return new Response<List<EventoDTOResponse>>(EventosResponse, "Eventos obtenidos con exito");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ha ocurrido un error en la consulta de los eventos {ex}");
            }
        }

        public async Task<Response<EventoDTOResponse>> GetById(int Id)
        {
            try
            {
                using (unitOfWork)
                {

                    var EventoRepositorio = unitOfWork.GetRepository<Evento>();
                    var Evento = await EventoRepositorio.FirstOrDefaultAsyncThenInclude(
                          evento => evento.EventoId.Equals(Id),
                          evento => evento
                          .Include(e => e.Cliente));
                    if (Evento == null)
                    {
                        return new Response<EventoDTOResponse>($"No se encontro informacion con el id {Id}", HttpStatusCode.NotFound);
                    }                  
                    var EventoResponse = mapper.Map<EventoDTOResponse>(Evento);
                    return new Response<EventoDTOResponse>(EventoResponse, "Evento obtenido con exito");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ha ocurrido un error al optener el dato {Id}, {ex}");
            }
        }

        public async Task<Response<bool>> UpdateAsync(EventoDTOUpdateRequest UpdateRequest)
        {
            try
            {
                using (unitOfWork)
                {
                    var EventoRepository = unitOfWork.GetRepository<Evento>();
                    var EventoExistente = await EventoRepository.FirstOrDefaultAsync(
                                            Evento => Evento.EventoId.Equals(UpdateRequest.EventoId));

                    if (EventoExistente == null)
                    {
                        return new Response<bool>($"No se encontró ningún Evento con el ID {UpdateRequest.EventoId}", HttpStatusCode.NotFound);
                    }

                    // Actualizar propiedades del Evento con los datos proporcionados
                    mapper.Map(UpdateRequest, EventoExistente);

                    await unitOfWork.SaveChangesAsync();

                    return new Response<bool>(true, "Evento actualizado correctamente");
                }

            }
            catch (Exception ex)
            {
                throw new Exception($"Ocurrió un error durante la actualización del Evento: {ex.Message}");
            }
        }

        public async Task<Response<List<EventoDTOResponse>>> GetAllActiveEvents()
        {
            try
            {
                using (unitOfWork)
                {
                    var EventoRepository = unitOfWork.GetRepository<Evento>();
                    
                    // Obtener eventos activos con relación de cliente incluida
                    var Eventos = await EventoRepository.FindAllAsync(
                        evento => evento.Estado == true,
                        evento => evento.Cliente!
                    );

                    if (Eventos.Count == 0)
                    {
                        return new Response<List<EventoDTOResponse>>("No se encontraron eventos", HttpStatusCode.NotFound);
                    }
                    
                    // Filtrar eventos futuros y mappear
                    var EventosResponse = Eventos
                        .Where(e => e.FechaInicio >= DateTime.UtcNow)
                        .Select(Evento => mapper.Map<EventoDTOResponse>(Evento))
                        .ToList();
                    
                    return new Response<List<EventoDTOResponse>>(EventosResponse, "Eventos obtenidos con exito");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ha ocurrido un error en la consulta de los eventos {ex}");
            }
        }
    }
}
