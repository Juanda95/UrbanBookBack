using Application.DTOs.Request;
using Application.DTOs.Response;
using Application.Helpers.Wrappers;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.Entities.DLandingPage;
using Microsoft.EntityFrameworkCore;
using Persistence.UnitOfWork.Interface;
using System.Net;

namespace Application.Services
{
    public class LandingConfigService(IUnitOfWork unitOfWork, IMapper mapper) : ILandingConfigService
    {
        public async Task<Response<LandingConfigDTOResponse>> GetLandingConfig()
        {
            try
            {
                using (unitOfWork)
                {
                    var configRepo = unitOfWork.GetRepository<LandingConfig>();
                    var config = await configRepo.AsQueryable()
                        .Include(c => c.LandingServices.OrderBy(s => s.Orden))
                        .Include(c => c.LandingGalleryItems.OrderBy(g => g.Orden))
                        .FirstOrDefaultAsync(c => c.Activo);

                    if (config == null)
                    {
                        return new Response<LandingConfigDTOResponse>("No se encontro configuracion de landing", HttpStatusCode.NotFound);
                    }

                    var response = mapper.Map<LandingConfigDTOResponse>(config);
                    return new Response<LandingConfigDTOResponse>(response, "Configuracion obtenida con exito");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener la configuracion del landing: {ex}");
            }
        }

        public async Task<Response<LandingConfigDTOResponse>> UpsertLandingConfig(LandingConfigDTORequest request)
        {
            try
            {
                using (unitOfWork)
                {
                    var configRepo = unitOfWork.GetRepository<LandingConfig>();
                    var serviceRepo = unitOfWork.GetRepository<LandingService>();
                    var galleryRepo = unitOfWork.GetRepository<LandingGalleryItem>();

                    var existingConfig = await configRepo.AsQueryable()
                        .Include(c => c.LandingServices)
                        .Include(c => c.LandingGalleryItems)
                        .FirstOrDefaultAsync(c => c.Activo);

                    if (existingConfig != null)
                    {
                        // Update hero fields
                        existingConfig.HeroTitle = request.HeroTitle;
                        existingConfig.HeroSubtitle = request.HeroSubtitle;
                        existingConfig.HeroImageFileName = request.HeroImageFileName;
                        existingConfig.WhatsAppNumber = request.WhatsAppNumber;
                        existingConfig.WhatsAppMessage = request.WhatsAppMessage;
                        existingConfig.FechaModificacion = DateTime.UtcNow;

                        // Sync services: delete removed, update existing, add new
                        var requestServiceIds = request.Services
                            .Where(s => s.LandingServiceId.HasValue)
                            .Select(s => s.LandingServiceId!.Value)
                            .ToHashSet();

                        var servicesToRemove = existingConfig.LandingServices
                            .Where(s => !requestServiceIds.Contains(s.LandingServiceId))
                            .ToList();
                        serviceRepo.Delete(servicesToRemove);

                        foreach (var svcReq in request.Services)
                        {
                            if (svcReq.LandingServiceId.HasValue)
                            {
                                var existingSvc = existingConfig.LandingServices
                                    .FirstOrDefault(s => s.LandingServiceId == svcReq.LandingServiceId.Value);
                                if (existingSvc != null)
                                {
                                    existingSvc.Orden = svcReq.Orden;
                                    existingSvc.IconCode = svcReq.IconCode;
                                    existingSvc.Titulo = svcReq.Titulo;
                                    existingSvc.Descripcion = svcReq.Descripcion;
                                }
                            }
                            else
                            {
                                var newSvc = mapper.Map<LandingService>(svcReq);
                                newSvc.LandingConfigId = existingConfig.LandingConfigId;
                                serviceRepo.Insert(newSvc);
                            }
                        }

                        // Sync gallery items
                        var requestGalleryIds = request.GalleryItems
                            .Where(g => g.LandingGalleryItemId.HasValue)
                            .Select(g => g.LandingGalleryItemId!.Value)
                            .ToHashSet();

                        var galleryToRemove = existingConfig.LandingGalleryItems
                            .Where(g => !requestGalleryIds.Contains(g.LandingGalleryItemId))
                            .ToList();
                        galleryRepo.Delete(galleryToRemove);

                        foreach (var galReq in request.GalleryItems)
                        {
                            if (galReq.LandingGalleryItemId.HasValue)
                            {
                                var existingGal = existingConfig.LandingGalleryItems
                                    .FirstOrDefault(g => g.LandingGalleryItemId == galReq.LandingGalleryItemId.Value);
                                if (existingGal != null)
                                {
                                    existingGal.Orden = galReq.Orden;
                                    existingGal.MediaType = galReq.MediaType;
                                    existingGal.FileName = galReq.FileName;
                                    existingGal.AltText = galReq.AltText;
                                }
                            }
                            else
                            {
                                var newGal = mapper.Map<LandingGalleryItem>(galReq);
                                newGal.LandingConfigId = existingConfig.LandingConfigId;
                                galleryRepo.Insert(newGal);
                            }
                        }

                        await unitOfWork.SaveChangesAsync();

                        var updatedResponse = mapper.Map<LandingConfigDTOResponse>(existingConfig);
                        return new Response<LandingConfigDTOResponse>(updatedResponse, "Configuracion actualizada con exito");
                    }
                    else
                    {
                        // Create new config
                        var newConfig = mapper.Map<LandingConfig>(request);
                        configRepo.Insert(newConfig);
                        await unitOfWork.SaveChangesAsync();

                        var createdResponse = mapper.Map<LandingConfigDTOResponse>(newConfig);
                        return new Response<LandingConfigDTOResponse>(createdResponse, "Configuracion creada con exito", HttpStatusCode.Created);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al guardar la configuracion del landing: {ex}");
            }
        }
    }
}
