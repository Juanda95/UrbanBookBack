using Application.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Scheduler.Services
{
    public class ReminderSchedulerBackgroundService(IServiceScopeFactory scopeFactory, ILogger<ReminderSchedulerBackgroundService> logger) : BackgroundService
    {
        private readonly ILogger<ReminderSchedulerBackgroundService> _logger = logger;
        private readonly IServiceScopeFactory _scopeFactory = scopeFactory;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        _logger.LogInformation("Starting new iteration of ReminderSchedulerService.");
                        using var scope = _scopeFactory.CreateScope();
                        var configBackGroundService = scope.ServiceProvider.GetRequiredService<IConfigBackGroundService>();

                        var Parameter = await configBackGroundService.GetReminderIntervalConfig();
                        bool isReminderEnabled = Parameter?.Values?.FirstOrDefault(v => v.Code.Equals("ReminderEnabled"))?.Name == "true";
                        int intervalInMinutesConsult = int.TryParse(Parameter?.Values?.FirstOrDefault(v => v.Code.Equals("ReminderConsultIntervalMinutes"))?.Name, out int result) ? result : 30;
                        int intervalInMiliSecondsConsult = ConvertMinutesToMilliseconds(intervalInMinutesConsult);

                        if (isReminderEnabled)
                        {
                            _logger.LogInformation("Reminder service is enabled.");
                            var eventBackGroundService = scope.ServiceProvider.GetRequiredService<IEventBackGroundService>();
                            int intervalInMinutes = await configBackGroundService.GetReminderUpdaterInterval();
                            int intervalInMiliSecondsExecution = ConvertMinutesToMilliseconds(intervalInMinutes);
                            await eventBackGroundService.ScheduleEventReminders();
                            await Task.Delay(intervalInMiliSecondsExecution, stoppingToken);
                        }
                        else
                        {
                            _logger.LogInformation("Reminder service is disabled.");
                            await Task.Delay(intervalInMiliSecondsConsult, stoppingToken);
                        }


                    }
                    catch (TaskCanceledException ex)
                    {
                        _logger.LogInformation($"Task was cancelled. {ex.Message}");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "An error occurred in the ReminderSchedulerService.");
                        await Task.Delay(1800000, stoppingToken);
                    }
                }
            }
            catch (TaskCanceledException)
            {
                _logger.LogInformation("Task was cancelled.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the ReminderSchedulerService.");

            }
        }

        private static int ConvertMinutesToMilliseconds(int minutes)
        {
            return minutes * 60 * 1000;
        }

    }
}
