using Domain.Interfaces;

namespace Application.Services
{
    /// <summary>
    /// Servicio scoped que almacena el TenantId del request actual.
    /// TenantId == 0 indica modo admin/sin tenant (ve todos los datos).
    /// </summary>
    public class TenantService : ITenantService
    {
        private int _tenantId;
        private string _tenantSlug = string.Empty;

        public int GetCurrentTenantId() => _tenantId;
        public string GetCurrentTenantSlug() => _tenantSlug;

        public void SetTenant(int tenantId, string slug)
        {
            _tenantId = tenantId;
            _tenantSlug = slug;
        }
    }
}
