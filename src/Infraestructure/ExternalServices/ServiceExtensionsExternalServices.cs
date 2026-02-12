using Application.Services.Interfaces;
using ExternalServices.Services;
using ExternalServices.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace ExternalServices
{
    public static class ServiceExtensionsExternalServices
    {
        public static void AddExternalServices(this IServiceCollection services, string UriWhatsAppApi)
        {
            services.AddSingleton<IWhatsAppService, WhatsAppService>(); 
            services.AddSingleton<IUtils, Util>();
            services.AddHttpClient("WhatsAppApi", client =>
            {
                client.BaseAddress = new Uri(UriWhatsAppApi);
            });
            services.AddHttpClient();

        }

    }
}
