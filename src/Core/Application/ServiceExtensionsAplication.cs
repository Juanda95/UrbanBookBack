using Application.DTOs.Request;
using Application.DTOs.Response;
using Application.Services;
using Application.Services.BackGroundService;
using Application.Services.Calendario;
using Application.Services.Interfaces;
using Application.Services.Interfaces.Calendario;
using Application.Services.Interfaces.Messaging;
using Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application
{
    public static class ServiceExtensionsAplication
    {
        public static void AddAplicationLayer(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddScoped<ITenantService, TenantService>();
            services.AddScoped<ICommonService<UsuarioDTOResponse, UsuarioDTORequest, UsuarioDTOUpdateRequest>, UsuarioService>();
            services.AddScoped<ICommonService<ClienteDTOResponse, ClienteDTORequest, ClienteDTOUpdateRequest>, ClienteService>();
            services.AddScoped<IEventsService, EventoServices>();
            services.AddScoped<IPerfilService, PerfilService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IConfigBackGroundService,DataBaseBackGroundService>();
            services.AddScoped<IParameterService, ParameterService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IStateProcessEventsService, StateProcessEventsService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IServicioService, ServicioService>();
            services.AddScoped<IPublicBookingService, PublicBookingService>();
            services.AddScoped<IHorarioAtencionService, HorarioAtencionService>();
            services.AddScoped<INegocioService, NegocioService>();
            services.AddScoped<ILandingConfigService, LandingConfigService>();
            services.AddScoped<IDashboardService, DashboardService>();
        }
    }
}
