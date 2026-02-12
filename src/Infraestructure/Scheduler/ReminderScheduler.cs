using Application.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Quartz;
using Scheduler.Jobs;

namespace Scheduler
{
    public class ReminderScheduler(ISchedulerFactory schedulerFactory, IConfigBackGroundService configBackGroundService, ILogger<ReminderScheduler> logger)
    {
        private readonly IScheduler _scheduler = schedulerFactory.GetScheduler().Result;
        private readonly IConfigBackGroundService _ConfigBackGroundService = configBackGroundService;
        private readonly ILogger<ReminderScheduler> _logger = logger;

        public async Task ScheduleReminder(int eventId, DateTime eventStartTime, TimeSpan reminderOffset)
        {
            _logger.LogInformation($"info = {eventId}, {eventStartTime},{reminderOffset}------------------------------------------------------");
            var job = JobBuilder.Create<ReminderJob>()
                .WithIdentity($"reminder_{eventId}_{reminderOffset}")
                .UsingJobData("EventId", eventId)
                .Build();

            var trigger = TriggerBuilder.Create()
                .WithIdentity($"trigger_{eventId}_{reminderOffset}")
                .StartAt(eventStartTime - reminderOffset)
                .Build();

            await _scheduler.ScheduleJob(job, trigger);
        }

    }
}
