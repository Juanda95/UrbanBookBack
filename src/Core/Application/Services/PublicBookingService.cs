using Application.DTOs.Request;
using Application.DTOs.Request.Messaging;
using Application.DTOs.Response;
using Application.Helpers.Wrappers;
using Application.Services.Interfaces;
using Application.Services.Interfaces.Messaging;
using AutoMapper;
using Domain.Entities.DCalendario;
using Domain.Entities.Dcliente;
using Domain.Entities.DServicio;
using Domain.Entities.DUsuario;
using Persistence.UnitOfWork.Interface;
using System.Net;

namespace Application.Services
{
    public class PublicBookingService(IUnitOfWork unitOfWork, IMapper mapper, IEmailService emailService) : IPublicBookingService
    {
        private readonly IEmailService _emailService = emailService;

        public async Task<Response<List<ProfessionalPublicDTOResponse>>> GetPublicProfessionals()
        {
            try
            {
                using (unitOfWork)
                {
                    var usuarioRepository = unitOfWork.GetRepository<Usuario>();
                    var usuarios = await usuarioRepository.GetAllAsync();

                    if (!usuarios.Any())
                    {
                        return new Response<List<ProfessionalPublicDTOResponse>>("No se encontraron profesionales", HttpStatusCode.NotFound);
                    }

                    var professionalsResponse = usuarios.Select(u => mapper.Map<ProfessionalPublicDTOResponse>(u)).ToList();
                    return new Response<List<ProfessionalPublicDTOResponse>>(professionalsResponse, "Profesionales obtenidos con éxito");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ha ocurrido un error al consultar los profesionales {ex}");
            }
        }

        public async Task<Response<List<TimeSlotDTOResponse>>> GetAvailableSlots(int usuarioId, DateTime fecha, int duracionMinutos)
        {
            try
            {
                using (unitOfWork)
                {
                    // Validar que el profesional exista
                    var usuarioRepository = unitOfWork.GetRepository<Usuario>();
                    var usuario = await usuarioRepository.FirstOrDefaultAsync(u => u.UsuarioId == usuarioId);
                    if (usuario == null)
                    {
                        return new Response<List<TimeSlotDTOResponse>>($"No se encontró el profesional con ID {usuarioId}", HttpStatusCode.NotFound);
                    }

                    // Obtener eventos del profesional para la fecha indicada
                    // Nota: 'fecha' llega como medianoche local del usuario en UTC
                    // (ej: Colombia UTC-5: 2026-02-17T05:00:00Z = medianoche local del 17 Feb)
                    // Usamos 'fecha' directamente como base para preservar el contexto de timezone
                    var eventoRepository = unitOfWork.GetRepository<Evento>();
                    var fechaInicioDia = fecha;
                    var fechaFinDia = fecha.AddDays(1);

                    var eventosDelDia = await eventoRepository.FindAllAsync(
                        e => e.UsuarioId == usuarioId &&
                             e.Estado == true &&
                             e.FechaInicio >= fechaInicioDia &&
                             e.FechaInicio < fechaFinDia
                    );

                    // Obtener horario configurado del profesional para el día de la semana
                    var horarioRepository = unitOfWork.GetRepository<HorarioAtencion>();
                    var exclusionRepository = unitOfWork.GetRepository<ExclusionHorario>();
                    int diaSemana = (int)fecha.DayOfWeek;

                    var horariosDelDiaSemana = await horarioRepository.FindAllAsync(
                        h => h.UsuarioId == usuarioId && h.DiaSemana == diaSemana);
                    var horarioDia = horariosDelDiaSemana.FirstOrDefault();

                    // Valores por defecto si no hay configuración
                    TimeSpan horaInicioDia;
                    TimeSpan horaFinDia;
                    bool diaActivo = true;
                    var exclusionesHorario = new List<ExclusionHorario>();

                    if (horarioDia != null)
                    {
                        diaActivo = horarioDia.Activo;
                        horaInicioDia = horarioDia.HoraInicio;
                        horaFinDia = horarioDia.HoraFin;

                        if (diaActivo)
                        {
                            var exclusiones = await exclusionRepository.FindAllAsync(
                                e => e.HorarioAtencionId == horarioDia.HorarioAtencionId);
                            exclusionesHorario = exclusiones.ToList();
                        }
                    }
                    else
                    {
                        // Fallback: horario por defecto 9:00 - 18:00
                        horaInicioDia = new TimeSpan(9, 0, 0);
                        horaFinDia = new TimeSpan(18, 0, 0);
                    }

                    // Si el día está desactivado, retornar lista vacía
                    if (!diaActivo)
                    {
                        return new Response<List<TimeSlotDTOResponse>>(
                            new List<TimeSlotDTOResponse>(),
                            "El profesional no atiende este día de la semana");
                    }

                    var intervaloMinutos = 30;
                    var slots = new List<TimeSlotDTOResponse>();

                    var currentSlotTime = horaInicioDia;
                    while (currentSlotTime < horaFinDia)
                    {
                        // Usar 'fecha' (medianoche local en UTC) como base para generar slots
                        // en hora local del negocio correctamente
                        var slotInicio = fecha.Add(currentSlotTime);
                        var slotFin = slotInicio.AddMinutes(duracionMinutos);
                        var slotFinTimeSpan = TimeSpan.FromMinutes(currentSlotTime.TotalMinutes + duracionMinutos);

                        // No crear slots que excedan el horario laboral
                        if (slotFinTimeSpan > horaFinDia)
                        {
                            currentSlotTime = currentSlotTime.Add(TimeSpan.FromMinutes(intervaloMinutos));
                            continue;
                        }

                        // No crear slots en el pasado
                        if (slotInicio <= DateTime.UtcNow)
                        {
                            currentSlotTime = currentSlotTime.Add(TimeSpan.FromMinutes(intervaloMinutos));
                            continue;
                        }

                        // Verificar si el inicio del slot cae dentro de una exclusión (ej: almuerzo)
                        // Solo se bloquean slots cuyo INICIO esté dentro del rango de exclusión
                        // Ej: exclusión 12:00-13:00 bloquea slots de 12:00 y 12:30, pero permite 11:30 y 13:00
                        var slotInicioTs = currentSlotTime;
                        var enExclusion = exclusionesHorario.Any(ex =>
                            slotInicioTs >= ex.HoraInicio && slotInicioTs < ex.HoraFin);

                        if (enExclusion)
                        {
                            currentSlotTime = currentSlotTime.Add(TimeSpan.FromMinutes(intervaloMinutos));
                            continue;
                        }

                        // Verificar si el slot tiene conflicto con eventos existentes
                        var tieneConflicto = eventosDelDia.Any(e =>
                            (slotInicio < e.FechaFin && slotFin > e.FechaInicio));

                        slots.Add(new TimeSlotDTOResponse
                        {
                            FechaInicio = slotInicio,
                            FechaFin = slotFin,
                            Disponible = !tieneConflicto
                        });

                        currentSlotTime = currentSlotTime.Add(TimeSpan.FromMinutes(intervaloMinutos));
                    }

                    if (!slots.Any())
                    {
                        return new Response<List<TimeSlotDTOResponse>>("No hay horarios disponibles para la fecha seleccionada", HttpStatusCode.NotFound);
                    }

                    return new Response<List<TimeSlotDTOResponse>>(slots, "Horarios obtenidos con éxito");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ha ocurrido un error al consultar los horarios disponibles {ex}");
            }
        }

        public async Task<Response<ClienteDTOResponse>> PublicLogin(PublicLoginDTORequest request)
        {
            try
            {
                using (unitOfWork)
                {
                    // Validar que al menos correo o teléfono estén presentes
                    if (string.IsNullOrWhiteSpace(request.Correo) && string.IsNullOrWhiteSpace(request.Telefono))
                    {
                        return new Response<ClienteDTOResponse>("Debe proporcionar correo o teléfono", HttpStatusCode.BadRequest);
                    }

                    var clienteRepository = unitOfWork.GetRepository<Cliente>();

                    // Buscar cliente por (correo OR teléfono) AND documento
                    Cliente? cliente = null;

                    if (!string.IsNullOrWhiteSpace(request.Correo))
                    {
                        var clientes = await clienteRepository.FindAllAsync(
                            c => c.Correo == request.Correo && c.NumeroDocumento == request.NumeroDocumento
                        );
                        cliente = clientes.FirstOrDefault();
                    }

                    if (cliente == null && !string.IsNullOrWhiteSpace(request.Telefono))
                    {
                        var clientes = await clienteRepository.FindAllAsync(
                            c => c.Telefono == request.Telefono && c.NumeroDocumento == request.NumeroDocumento
                        );
                        cliente = clientes.FirstOrDefault();
                    }

                    if (cliente == null)
                    {
                        return new Response<ClienteDTOResponse>("No se encontró un cliente con los datos proporcionados", HttpStatusCode.NotFound);
                    }

                    var clienteResponse = mapper.Map<ClienteDTOResponse>(cliente);
                    return new Response<ClienteDTOResponse>(clienteResponse, "Inicio de sesión exitoso");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ha ocurrido un error en el inicio de sesión {ex}");
            }
        }

        public async Task<Response<ClienteDTOResponse>> PublicRegister(ClienteDTORequest request)
        {
            try
            {
                using (unitOfWork)
                {
                    var clienteRepository = unitOfWork.GetRepository<Cliente>();

                    // Validar unicidad del correo
                    var clientesCorreo = await clienteRepository.FindAllAsync(c => c.Correo == request.Correo);
                    if (clientesCorreo.Any())
                    {
                        return new Response<ClienteDTOResponse>("Ya existe un cliente registrado con este correo electrónico", HttpStatusCode.Conflict);
                    }

                    // Validar unicidad del número de documento
                    var clientesDocumento = await clienteRepository.FindAllAsync(c => c.NumeroDocumento == request.NumeroDocumento);
                    if (clientesDocumento.Any())
                    {
                        return new Response<ClienteDTOResponse>("Ya existe un cliente registrado con este número de documento", HttpStatusCode.Conflict);
                    }

                    Cliente nuevoCliente = mapper.Map<Cliente>(request);
                    clienteRepository.Insert(nuevoCliente);
                    await unitOfWork.SaveChangesAsync();

                    var clienteResponse = mapper.Map<ClienteDTOResponse>(nuevoCliente);
                    return new Response<ClienteDTOResponse>(clienteResponse, "Registro exitoso", HttpStatusCode.Created);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ha ocurrido un error en el registro del cliente {ex}");
            }
        }

        public async Task<Response<EventoDTOResponse>> CreatePublicBooking(PublicBookingDTORequest request)
        {
            try
            {
                // Validación: FechaInicio < FechaFin
                if (request.FechaInicio >= request.FechaFin)
                {
                    return new Response<EventoDTOResponse>("La fecha de fin debe ser posterior a la de inicio", HttpStatusCode.BadRequest);
                }

                // Validación: No en el pasado
                if (request.FechaInicio < DateTime.UtcNow)
                {
                    return new Response<EventoDTOResponse>("No puedes agendar citas en el pasado", HttpStatusCode.BadRequest);
                }

                // Validación: Duración mínima 30 minutos
                var duracion = request.FechaFin - request.FechaInicio;
                if (duracion.TotalMinutes < 30)
                {
                    return new Response<EventoDTOResponse>("La cita debe tener una duración mínima de 30 minutos", HttpStatusCode.BadRequest);
                }

                // Validación: No más de 6 meses en el futuro
                if (request.FechaInicio > DateTime.UtcNow.AddMonths(6))
                {
                    return new Response<EventoDTOResponse>("No puedes agendar citas con más de 6 meses de anticipación", HttpStatusCode.BadRequest);
                }

                using (unitOfWork)
                {
                    // Validar que el cliente exista
                    var clienteRepository = unitOfWork.GetRepository<Cliente>();
                    var cliente = await clienteRepository.FirstOrDefaultAsync(c => c.clienteId.Equals(request.ClienteId));
                    if (cliente == null)
                    {
                        return new Response<EventoDTOResponse>($"El cliente {request.ClienteId} no se encuentra registrado", HttpStatusCode.NotFound);
                    }

                    // Validar que el profesional exista
                    var usuarioRepository = unitOfWork.GetRepository<Usuario>();
                    var usuario = await usuarioRepository.FirstOrDefaultAsync(u => u.UsuarioId == request.UsuarioId);
                    if (usuario == null)
                    {
                        return new Response<EventoDTOResponse>($"El profesional {request.UsuarioId} no se encuentra registrado", HttpStatusCode.NotFound);
                    }

                    // Validar que el servicio exista
                    var servicioRepository = unitOfWork.GetRepository<Servicio>();
                    var servicio = await servicioRepository.FirstOrDefaultAsync(s => s.ServicioId == request.ServicioId);
                    if (servicio == null)
                    {
                        return new Response<EventoDTOResponse>($"El servicio {request.ServicioId} no se encuentra registrado", HttpStatusCode.NotFound);
                    }

                    // Asignar StateProcessEventId por defecto (primer estado disponible)
                    var stateProcessRepository = unitOfWork.GetRepository<StateProcessEvents>();
                    var estadosProceso = await stateProcessRepository.GetAllAsync();
                    if (!estadosProceso.Any())
                    {
                        return new Response<EventoDTOResponse>(
                            "No hay estados de proceso disponibles. Por favor, contacta al administrador.",
                            HttpStatusCode.BadRequest);
                    }
                    var estadoInicial = estadosProceso.First().StateProcessEventsId;

                    // Verificar conflictos de horarios
                    var eventoRepository = unitOfWork.GetRepository<Evento>();
                    var eventoEnConflicto = await eventoRepository.FirstOrDefaultAsync(e =>
                        e.UsuarioId == request.UsuarioId &&
                        e.Estado == true &&
                        ((request.FechaInicio < e.FechaFin && request.FechaFin > e.FechaInicio) ||
                        (e.FechaInicio < request.FechaFin && e.FechaFin > request.FechaInicio)));

                    if (eventoEnConflicto != null)
                    {
                        return new Response<EventoDTOResponse>("Ya existe una cita en la franja horaria seleccionada", HttpStatusCode.Conflict);
                    }

                    // Crear el evento
                    var nuevoEvento = new Evento
                    {
                        ClienteId = request.ClienteId,
                        UsuarioId = request.UsuarioId,
                        Titulo = servicio.Nombre,
                        Descripcion = $"Reserva pública - {servicio.Nombre}",
                        Estado = true,
                        StateProcessEventId = estadoInicial,
                        FechaInicio = request.FechaInicio,
                        FechaFin = request.FechaFin,
                        FechaRegistro = DateTime.UtcNow,
                        IsScheduled = true
                    };

                    eventoRepository.Insert(nuevoEvento);
                    await unitOfWork.SaveChangesAsync();

                    // Enviar email de confirmación
                    try
                    {
                        string htmlBody = GenerarEmailConfirmacion(nuevoEvento, cliente, servicio);
                        await _emailService.SendEmailAsync(new SendEmailDTORequest
                        {
                            SmtpConfigId = 2,
                            To = cliente.Correo,
                            Subject = $"Confirmación de Cita - {servicio.Nombre}",
                            Body = htmlBody
                        });
                    }
                    catch (Exception emailEx)
                    {
                        Console.WriteLine($"Error al enviar email de confirmación: {emailEx.Message}");
                    }

                    var eventoResponse = mapper.Map<EventoDTOResponse>(nuevoEvento);
                    return new Response<EventoDTOResponse>(eventoResponse, "Reserva creada con éxito", HttpStatusCode.Created);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error detallado en CreatePublicBooking: {ex.Message} | {ex.StackTrace}");
                return new Response<EventoDTOResponse>(
                    "Error al crear la reserva. Por favor intenta más tarde.",
                    HttpStatusCode.InternalServerError);
            }
        }

        private string GenerarEmailConfirmacion(Evento evento, Cliente cliente, Servicio servicio)
        {
            return $@"<!DOCTYPE html>
            <html lang='es'>
            <head>
                <meta charset='UTF-8'>
                <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                <title>Confirmación de Reserva</title>
            </head>
            <body style='font-family: Arial, sans-serif; color: #333; background-color: #f6f8fc; padding: 20px;'>
                <table style='max-width: 600px; margin: 0 auto; border: 1px solid #ddd; border-radius: 10px; background-color: #fff;'>
                    <tr>
                        <td style='padding: 20px; text-align: center;'>
                            <h2 style='color: #3b82f6; margin: 0;'>Confirmación de Reserva</h2>
                        </td>
                    </tr>
                    <tr>
                        <td style='padding: 0 20px 20px 20px;'>
                            <p>Estimado/a {cliente.Nombres} {cliente.PrimerApellido},</p>
                            <p>Tu reserva ha sido creada exitosamente. Aquí están los detalles:</p>
                            <div style='background-color: #f0f0f0; padding: 15px; border-radius: 5px; margin: 20px 0;'>
                                <p><strong>Servicio:</strong> {servicio.Nombre}</p>
                                <p><strong>Duración:</strong> {servicio.DuracionMinutos} minutos</p>
                                <p><strong>Fecha:</strong> {evento.FechaInicio:dd/MM/yyyy}</p>
                                <p><strong>Hora:</strong> {evento.FechaInicio:HH:mm} - {evento.FechaFin:HH:mm}</p>
                            </div>
                            <p>Si tienes alguna pregunta o necesitas modificar tu cita, no dudes en contactarnos.</p>
                            <p>Gracias por tu confianza.</p>
                            <p>Saludos cordiales,<br>UrbanBook</p>
                            <hr style='border: none; border-top: 1px solid #ddd; margin-top: 20px;'>
                            <p style='font-size: 12px; color: #777;'>Este correo fue generado automáticamente, por favor no responda a este mensaje.</p>
                        </td>
                    </tr>
                </table>
            </body>
            </html>";
        }
    }
}
