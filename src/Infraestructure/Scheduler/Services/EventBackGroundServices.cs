using Application.Services.Interfaces;
using Domain.Entities.DCalendario;
using Domain.Entities.Parametros;
using Microsoft.Extensions.Logging;
using Persistence.UnitOfWork.Interface;

namespace Scheduler.Services
{
    public class EventBackGroundServices(IUnitOfWork unitOfWork, ILogger<EventBackGroundServices> logger, ReminderScheduler reminderScheduler) : IEventBackGroundService
    {
        private readonly ILogger<EventBackGroundServices> _logger = logger;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ReminderScheduler _reminderScheduler = reminderScheduler;

        public async Task ScheduleEventReminders()
        {
            var eventsRepository = _unitOfWork.GetRepository<Evento>();
            var reminderConfigurationsRepository = _unitOfWork.GetRepository<Parameter>();

            // Obtener la fecha y hora actual
            var currentDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "SA Pacific Standard Time");
            // Filtrar eventos que no han sido programados y cuya fecha de inicio es mayor a la fecha actual
            _logger.LogInformation($"Zona hora colombia {currentDate}");
            var events = await eventsRepository.FindAllAsync(e => !e.IsScheduled && e.FechaInicio > currentDate);

            if (events.Count == 0)
            {
                _logger.LogInformation("No hay eventos para programar.");
                return;
            }

            var reminderConfigurations = await reminderConfigurationsRepository.FirstOrDefaultAsync(
                                              parametro => parametro.TypeParameter.Equals("ReminderConfigureTimeBefore"),
                                              parameter => parameter.Values);

            if (reminderConfigurations == null)
            {
                _logger.LogWarning("No se encontraron configuraciones de recordatorio.");
                return;
            }         

            foreach (var ev in events)
            {
                foreach (var config in reminderConfigurations.Values)
                {
                    if (int.TryParse(config.Name, out int hours))
                    {
                        TimeSpan timeBefore = TimeSpan.FromHours(hours);
                        await _reminderScheduler.ScheduleReminder(ev.EventoId, ev.FechaInicio, timeBefore);
                        _logger.LogInformation("armado de envio de eventos.");

                    }
                    else
                    {
                        _logger.LogWarning($"No se pudo convertir {config.Name} a TimeSpan.");
                    }
                }
                ev.IsScheduled = true;
                eventsRepository.Update(ev); 
            }

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
