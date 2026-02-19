using Domain.Interfaces;
using Domain.Entities.DNegocio;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Persistence.Contexts;

namespace UrbanBook.Handlers
{
    /// <summary>
    /// Middleware que resuelve el tenant a partir del header X-Tenant-Id inyectado por Nginx.
    /// Usa cache en memoria para evitar queries repetitivos a la BD.
    /// </summary>
    public class TenantResolutionMiddleware
    {
        private readonly RequestDelegate _next;

        public TenantResolutionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(
            HttpContext context,
            ITenantService tenantService,
            UrbanBookDbContext dbContext,
            IMemoryCache cache)
        {
            // 1. Leer header "X-Tenant-Id" inyectado por Nginx
            var tenantSlug = context.Request.Headers["X-Tenant-Id"].FirstOrDefault();

            // 2. Sin header = modo admin/sin tenant (TenantId queda en 0)
            if (string.IsNullOrEmpty(tenantSlug))
            {
                await _next(context);
                return;
            }

            // 3. Buscar en cache o BD
            var cacheKey = $"tenant:{tenantSlug}";
            if (!cache.TryGetValue(cacheKey, out Negocio? negocio))
            {
                negocio = await dbContext.Negocios
                    .AsNoTracking()
                    .FirstOrDefaultAsync(n => n.Slug == tenantSlug && n.Activo);

                if (negocio != null)
                {
                    cache.Set(cacheKey, negocio, TimeSpan.FromMinutes(5));
                }
            }

            // 4. Si no existe, retornar 404
            if (negocio == null)
            {
                context.Response.StatusCode = 404;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync("{\"succeeded\":false,\"message\":\"Negocio no encontrado\"}");
                return;
            }

            // 5. Setear el tenant en el servicio scoped
            tenantService.SetTenant(negocio.NegocioId, negocio.Slug);

            // 6. Validacion cruzada: si el usuario esta autenticado, verificar que su token corresponda al tenant
            if (context.User.Identity?.IsAuthenticated == true)
            {
                var tokenNegocioId = context.User.FindFirst("NegocioId")?.Value;
                if (tokenNegocioId != null && int.TryParse(tokenNegocioId, out int tokenTenantId) && tokenTenantId != negocio.NegocioId)
                {
                    context.Response.StatusCode = 403;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync("{\"succeeded\":false,\"message\":\"Token no corresponde a este negocio\"}");
                    return;
                }
            }

            // 7. Continuar con el pipeline
            await _next(context);
        }
    }
}
