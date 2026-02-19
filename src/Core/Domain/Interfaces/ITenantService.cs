namespace Domain.Interfaces
{
    /// <summary>
    /// Servicio scoped que almacena el TenantId del request actual.
    /// </summary>
    public interface ITenantService
    {
        int GetCurrentTenantId();
        string GetCurrentTenantSlug();
        void SetTenant(int tenantId, string slug);
    }
}
