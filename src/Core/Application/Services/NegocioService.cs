using Application.DTOs.Request;
using Application.DTOs.Response;
using Application.Helpers.Wrappers;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.Entities.DNegocio;
using Domain.Entities.DServicio;
using Domain.Entities.DUsuario;
using Persistence.UnitOfWork.Interface;
using System.Net;
using System.Text.RegularExpressions;

namespace Application.Services
{
    public class NegocioService(IUnitOfWork unitOfWork, IMapper mapper) : INegocioService
    {
        private static readonly Regex SlugRegex = new("^[a-z0-9]+(-[a-z0-9]+)*$", RegexOptions.Compiled);

        public async Task<Response<List<NegocioDTOResponse>>> GetAll()
        {
            try
            {
                using (unitOfWork)
                {
                    var repo = unitOfWork.GetRepository<Negocio>();
                    var negocios = await repo.GetAllAsync();

                    var response = negocios.Select(n =>
                    {
                        var dto = mapper.Map<NegocioDTOResponse>(n);
                        return dto;
                    }).ToList();

                    return new Response<List<NegocioDTOResponse>>(response, "Negocios obtenidos con exito");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener negocios: {ex}");
            }
        }

        public async Task<Response<NegocioDTOResponse>> GetById(int id)
        {
            try
            {
                using (unitOfWork)
                {
                    var repo = unitOfWork.GetRepository<Negocio>();
                    var negocio = await repo.FirstOrDefaultAsync(n => n.NegocioId == id);

                    if (negocio == null)
                        return new Response<NegocioDTOResponse>($"No se encontro negocio con id {id}", HttpStatusCode.NotFound);

                    var dto = mapper.Map<NegocioDTOResponse>(negocio);
                    return new Response<NegocioDTOResponse>(dto, "Negocio obtenido con exito");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener negocio: {ex}");
            }
        }

        public async Task<Response<NegocioDTOResponse>> GetBySlug(string slug)
        {
            try
            {
                using (unitOfWork)
                {
                    var repo = unitOfWork.GetRepository<Negocio>();
                    var negocio = await repo.FirstOrDefaultAsync(n => n.Slug == slug);

                    if (negocio == null)
                        return new Response<NegocioDTOResponse>($"No se encontro negocio con slug '{slug}'", HttpStatusCode.NotFound);

                    var dto = mapper.Map<NegocioDTOResponse>(negocio);
                    return new Response<NegocioDTOResponse>(dto, "Negocio obtenido con exito");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener negocio por slug: {ex}");
            }
        }

        public async Task<Response<NegocioDTOResponse>> CreateAsync(NegocioDTORequest request)
        {
            try
            {
                // Validar formato de slug
                if (!SlugRegex.IsMatch(request.Slug))
                    return new Response<NegocioDTOResponse>("El slug solo puede contener letras minusculas, numeros y guiones", HttpStatusCode.BadRequest);

                using (unitOfWork)
                {
                    var repo = unitOfWork.GetRepository<Negocio>();

                    // Validar slug unico
                    var existente = await repo.FirstOrDefaultAsync(n => n.Slug == request.Slug);
                    if (existente != null)
                        return new Response<NegocioDTOResponse>($"Ya existe un negocio con el slug '{request.Slug}'", HttpStatusCode.Conflict);

                    var negocio = mapper.Map<Negocio>(request);
                    negocio.FechaCreacion = DateTime.UtcNow;
                    negocio.Activo = true;

                    repo.Insert(negocio);
                    await unitOfWork.SaveChangesAsync();

                    // Crear servicios por defecto para el nuevo negocio
                    var servicioRepo = unitOfWork.GetRepository<Servicio>();
                    var serviciosDefault = new List<Servicio>
                    {
                        new() { Nombre = "Corte de Pelo", Descripcion = "Corte de pelo profesional con estilo personalizado", Precio = 25000, DuracionMinutos = 60, Activo = true, NegocioId = negocio.NegocioId },
                        new() { Nombre = "Corte de Pelo y Barba", Descripcion = "Corte de pelo y arreglo de barba profesional", Precio = 35000, DuracionMinutos = 90, Activo = true, NegocioId = negocio.NegocioId },
                        new() { Nombre = "Solo Barba", Descripcion = "Arreglo y perfilado de barba profesional", Precio = 15000, DuracionMinutos = 30, Activo = true, NegocioId = negocio.NegocioId }
                    };
                    foreach (var servicio in serviciosDefault)
                        servicioRepo.Insert(servicio);

                    await unitOfWork.SaveChangesAsync();

                    // Crear usuario administrador inicial si se proporcionaron los datos
                    if (!string.IsNullOrWhiteSpace(request.AdminEmail) &&
                        !string.IsNullOrWhiteSpace(request.AdminPassword) &&
                        !string.IsNullOrWhiteSpace(request.AdminNombre) &&
                        !string.IsNullOrWhiteSpace(request.AdminApellido))
                    {
                        var perfilRepo = unitOfWork.GetRepository<Perfil>();
                        var adminPerfil = await perfilRepo.FirstOrDefaultAsync(p => p.Rol == "admin");

                        if (adminPerfil != null)
                        {
                            var adminUsuario = new Usuario
                            {
                                Email = request.AdminEmail.Trim(),
                                Password = request.AdminPassword,
                                Nombre = request.AdminNombre.Trim(),
                                Apellido = request.AdminApellido.Trim(),
                                Direccion = request.Direccion ?? string.Empty,
                                Telefono = request.Telefono ?? string.Empty,
                                NegocioId = negocio.NegocioId,
                                Perfiles = [adminPerfil]
                            };

                            var usuarioRepo = unitOfWork.GetRepository<Usuario>();
                            usuarioRepo.Insert(adminUsuario);
                            await unitOfWork.SaveChangesAsync();
                        }
                    }

                    var dto = mapper.Map<NegocioDTOResponse>(negocio);
                    return new Response<NegocioDTOResponse>(dto, "Negocio creado con exito", HttpStatusCode.Created);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al crear negocio: {ex}");
            }
        }

        public async Task<Response<bool>> UpdateAsync(NegocioDTOUpdateRequest request)
        {
            try
            {
                if (!SlugRegex.IsMatch(request.Slug))
                    return new Response<bool>("El slug solo puede contener letras minusculas, numeros y guiones", HttpStatusCode.BadRequest);

                using (unitOfWork)
                {
                    var repo = unitOfWork.GetRepository<Negocio>();

                    var negocio = await repo.FirstOrDefaultAsync(n => n.NegocioId == request.NegocioId);
                    if (negocio == null)
                        return new Response<bool>($"No se encontro negocio con id {request.NegocioId}", HttpStatusCode.NotFound);

                    // Validar slug unico si cambio
                    if (negocio.Slug != request.Slug)
                    {
                        var existente = await repo.FirstOrDefaultAsync(n => n.Slug == request.Slug);
                        if (existente != null)
                            return new Response<bool>($"Ya existe un negocio con el slug '{request.Slug}'", HttpStatusCode.Conflict);
                    }

                    negocio.Nombre = request.Nombre;
                    negocio.Slug = request.Slug;
                    negocio.Descripcion = request.Descripcion;
                    negocio.LogoUrl = request.LogoUrl;
                    negocio.Telefono = request.Telefono;
                    negocio.Direccion = request.Direccion;
                    negocio.Correo = request.Correo;
                    negocio.Activo = request.Activo;

                    repo.Update(negocio);
                    await unitOfWork.SaveChangesAsync();

                    return new Response<bool>(true, "Negocio actualizado con exito");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al actualizar negocio: {ex}");
            }
        }

        public async Task<Response<bool>> DeactivateAsync(int id)
        {
            try
            {
                using (unitOfWork)
                {
                    var repo = unitOfWork.GetRepository<Negocio>();
                    var negocio = await repo.FirstOrDefaultAsync(n => n.NegocioId == id);

                    if (negocio == null)
                        return new Response<bool>($"No se encontro negocio con id {id}", HttpStatusCode.NotFound);

                    negocio.Activo = false;
                    repo.Update(negocio);
                    await unitOfWork.SaveChangesAsync();

                    return new Response<bool>(true, "Negocio desactivado con exito");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al desactivar negocio: {ex}");
            }
        }
    }
}
