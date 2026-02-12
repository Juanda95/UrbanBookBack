using Application.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Scheduler.Jobs
{
    public class ReminderJob(INotificationService notificationService, ILogger<ReminderJob> logger) : IJob
    {
        private readonly INotificationService _notificationService = notificationService;
        private readonly ILogger<ReminderJob> _logger = logger;
        public async Task  Execute(IJobExecutionContext context)
        {
            try
            {
                var eventId = context.JobDetail.JobDataMap.GetInt("EventId");
                await _notificationService.CheckAndSendNotifications(eventId);

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error executing ReminderJob for event: {ex.Message}");
            }
                        
        }

    }
}
