using Application.DTOs.Request;
using Application.DTOs.Request.Calendario;
using Application.DTOs.Response;
using Application.DTOs.Response.Calendario;
using AutoMapper;
using Domain.Entities.DCalendario;
using Domain.Entities.Dcliente;
using Domain.Entities.DLandingPage;
using Domain.Entities.DServicio;
using Domain.Entities.DNegocio;
using Domain.Entities.DUsuario;
using Domain.Entities.Parametros;

namespace Application.Helpers.Mappings
{
    public class Mapps : Profile
    {
        public Mapps()
        {
            
            CreateMap<UsuarioDTORequest, Usuario>();

            CreateMap<UsuarioDTOUpdateRequest, Usuario>()
                .ForMember(dest => dest.Password, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Password)));
                

            CreateMap<PerfilDTORequest, Perfil>();

            CreateMap<Usuario, UsuarioDTOResponse>();
                

            CreateMap<UsuarioDTOResponse,Usuario>();

            CreateMap<PerfilDTOResponse, Perfil>();

            CreateMap<Perfil, PerfilDTOResponse>();


            CreateMap<Cliente, ClienteDTOResponse> ();
            CreateMap<Cliente, ClienteEventoDTOResponse> ();
            CreateMap<ClienteDTORequest, Cliente> ();
            CreateMap<ClienteDTOUpdateRequest, Cliente>();

            CreateMap<EventoDTORequest, Evento>();
            CreateMap<Evento, EventoDTOResponse>();
            CreateMap<Evento, EventoHistoryDTOResponse>();
            CreateMap<EventoDTOUpdateRequest, Evento>();

            CreateMap<ParametersDTORequest, Parameter>();
            CreateMap<Parameter, ParameterDTOResponse>();
            CreateMap<ParameterDTOUpdateRequest, Parameter>();

            CreateMap<ValueDTORequest, Value>();
            CreateMap<Value, ValueDTOResponse>();

            CreateMap<StateProcessEventsDTORequest, StateProcessEvents>();
            CreateMap<StateProcessEvents, StateProcessEventsResponse>();

            // Servicio
            CreateMap<ServicioDTORequest, Servicio>();
            CreateMap<Servicio, ServicioDTOResponse>();

            // Professional public view (solo datos públicos)
            CreateMap<Usuario, ProfessionalPublicDTOResponse>();

            // HorarioAtencion
            CreateMap<HorarioAtencionDTORequest, HorarioAtencion>();
            CreateMap<HorarioAtencion, HorarioAtencionDTOResponse>();
            CreateMap<ExclusionHorarioDTORequest, ExclusionHorario>();
            CreateMap<ExclusionHorario, ExclusionHorarioDTOResponse>();

            // Negocio (Multi-tenancy)
            CreateMap<NegocioDTORequest, Negocio>();
            CreateMap<NegocioDTOUpdateRequest, Negocio>();
            CreateMap<Negocio, NegocioDTOResponse>();
            CreateMap<Negocio, TenantInfoDTOResponse>();

            // Landing page customization
            CreateMap<LandingConfigDTORequest, LandingConfig>()
                .ForMember(dest => dest.LandingServices, opt => opt.MapFrom(src => src.Services))
                .ForMember(dest => dest.LandingGalleryItems, opt => opt.MapFrom(src => src.GalleryItems));
            CreateMap<LandingConfig, LandingConfigDTOResponse>()
                .ForMember(dest => dest.Services, opt => opt.MapFrom(src => src.LandingServices))
                .ForMember(dest => dest.GalleryItems, opt => opt.MapFrom(src => src.LandingGalleryItems));

            CreateMap<LandingServiceDTORequest, LandingService>();
            CreateMap<LandingService, LandingServiceDTOResponse>();

            CreateMap<LandingGalleryItemDTORequest, LandingGalleryItem>();
            CreateMap<LandingGalleryItem, LandingGalleryItemDTOResponse>();

        }

    }
}
