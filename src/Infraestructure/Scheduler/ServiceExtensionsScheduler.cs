using Application.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Scheduler.Services;

namespace Scheduler
{
    public static class ServiceExtensionsScheduler
    {
        public static void AddSchedulerServices(this IServiceCollection services)
        {
            services.AddQuartz(q =>
            {
                q.UseSimpleTypeLoader();
                q.UseInMemoryStore();
            });
            services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
            services.AddScoped<ReminderScheduler>();

            services.AddHostedService<ReminderSchedulerBackgroundService>();
            services.AddScoped<IEventBackGroundService, EventBackGroundServices>();
        }
    }
}
